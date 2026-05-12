using Application.Common;
using Core;
using Domain.Abstraction;
using Domain.Enums;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Match.DeleteMatch;

internal sealed class DeleteMatchRequestHandler(
    IMatchRepository matchRepository,
    IUnitOfWork unitOfWork
    )
    : IRequestHandler<DeleteMatchRequest, Result<DeleteMatchResponse>>
{
    private readonly IMatchRepository _matchRepository = matchRepository ?? throw new ArgumentNullException(nameof(matchRepository));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<Result<DeleteMatchResponse>> Handle(DeleteMatchRequest request, CancellationToken cancellationToken)
    {
        var match = await _matchRepository.GetByPublicIdAsync(request.MatchPublicId, cancellationToken);
        if (match is null)
        {
            return Result<DeleteMatchResponse>.Failure(ApplicationErrors.NotFound);
        }

        if (match.Status == MatchStatus.Cancelled || match.Status == MatchStatus.Completed)
        {
            return Result<DeleteMatchResponse>.Failure(ApplicationErrors.InvalidOperation);
        }

        if (match.StartTime <= DateTime.UtcNow)
        {
            return Result<DeleteMatchResponse>.Failure(ApplicationErrors.InvalidOperation);
        }

        _matchRepository.Delete(match, cancellationToken);
        var isDeleted = await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<DeleteMatchResponse>.Success(new DeleteMatchResponse()
        {
            IsDeleted = isDeleted == 1,
        });
    }
}
