using Application.Common;
using Core;
using Domain.Abstraction;
using Domain.Enums;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Knockout.RecordKnockoutResult;

public sealed class RecordKnockoutResultRequestHandler(
    IKnockoutBracketRepository bracketRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RecordKnockoutResultRequest, Result<RecordKnockoutResultResponse>>
{
    private readonly IKnockoutBracketRepository _bracketRepository = bracketRepository ?? throw new ArgumentNullException(nameof(bracketRepository));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<Result<RecordKnockoutResultResponse>> Handle(
        RecordKnockoutResultRequest request,
        CancellationToken cancellationToken)
    {
        var bracket = await _bracketRepository.GetByPublicIdAsync(request.BracketPublicId!.Value, cancellationToken);

        if (bracket is null)
            return Result<RecordKnockoutResultResponse>.Failure(ApplicationErrors.NotFound);

        var match = bracket.Matches.FirstOrDefault(m => m.PublicId == request.MatchPublicId!.Value);

        if (match is null)
            return Result<RecordKnockoutResultResponse>.Failure(ApplicationErrors.NotFound);

        if (match.Status != KnockoutMatchStatus.Scheduled)
            return Result<RecordKnockoutResultResponse>.Failure(ApplicationErrors.KnockoutMatchNotScheduled);

        match.RecordResult(request.HomePoints!.Value, request.AwayPoints!.Value);

        // Napredovanje pobednika u sledeću rundu (osim za Final i ThirdPlace)
        bracket.AdvanceWinner(match.PublicId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<RecordKnockoutResultResponse>.Success(new RecordKnockoutResultResponse
        {
            MatchPublicId = match.PublicId,
            WinnerPublicId = match.WinnerPublicId!.Value
        });
    }
}
