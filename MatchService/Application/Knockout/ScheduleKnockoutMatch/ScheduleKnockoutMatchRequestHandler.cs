using Application.Common;
using Application.Knockout.GetKnockoutBracket;
using Core;
using Domain.Abstraction;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Knockout.ScheduleKnockoutMatch;

internal sealed class ScheduleKnockoutMatchRequestHandler(
    IKnockoutBracketRepository bracketRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ScheduleKnockoutMatchRequest, Result<ScheduleKnockoutMatchResponse>>
{
    public async Task<Result<ScheduleKnockoutMatchResponse>> Handle(
        ScheduleKnockoutMatchRequest request,
        CancellationToken cancellationToken)
    {
        var bracket = await bracketRepository.GetByMatchPublicIdAsync(request.MatchPublicId, cancellationToken);

        if (bracket is null)
            return Result<ScheduleKnockoutMatchResponse>.Failure(ApplicationErrors.NotFound);

        var match = bracket.Matches.First(m => m.PublicId == request.MatchPublicId);

        match.Schedule(request.ScheduledDateTime, request.StadiumPublicId);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ScheduleKnockoutMatchResponse>.Success(new ScheduleKnockoutMatchResponse
        {
            Match = new KnockoutMatchDto
            {
                PublicId = match.PublicId,
                Round = match.Round,
                MatchOrder = match.MatchOrder,
                HomeTeamPublicId = match.HomeTeamPublicId,
                HomeTeamName = match.HomeTeamName,
                AwayTeamPublicId = match.AwayTeamPublicId,
                AwayTeamName = match.AwayTeamName,
                HomePoints = match.HomePoints,
                AwayPoints = match.AwayPoints,
                WinnerPublicId = match.WinnerPublicId,
                StadiumPublicId = match.StadiumPublicId,
                ScheduledAt = match.ScheduledAt,
                Status = match.Status
            }
        });
    }
}
