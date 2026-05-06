using Application.Common;
using Application.Contracts;
using Core;
using Domain.Abstraction;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Match.CreateMatch;

public sealed class CreateMatchRequestHandler(
    IMatchRepository matchRepository,
    ITeamServiceClient teamServiceClient,
    IStadiumServiceClient stadiumServiceClient,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateMatchRequest, Result<CreateMatchResponse>>
{
    public async Task<Result<CreateMatchResponse>> Handle(
        CreateMatchRequest request,
        CancellationToken cancellationToken)
    {
        if (request.HomeTeamPublicId == request.AwayTeamPublicId)
            return Result<CreateMatchResponse>.Failure(ApplicationErrors.DifferentTeams);

        var homeTeam = await teamServiceClient.GetTeamByPublicIdAsync(request.HomeTeamPublicId, cancellationToken);
        var awayTeam = await teamServiceClient.GetTeamByPublicIdAsync(request.AwayTeamPublicId, cancellationToken);

        if (homeTeam is null || !homeTeam.Exists || awayTeam is null || !awayTeam.Exists)
            return Result<CreateMatchResponse>.Failure(ApplicationErrors.NotFound);

        var stadium = await stadiumServiceClient.GetStadiumByPublicIdAsync(request.StadiumPublicId, cancellationToken);

        if (stadium is null || !stadium.Exists)
            return Result<CreateMatchResponse>.Failure(ApplicationErrors.NotFound);

        var hasTeamConflict = await matchRepository.ExistsTeamConflictAsync(
            request.HomeTeamPublicId,
            request.AwayTeamPublicId,
            request.StartTime,
            cancellationToken);

        if (hasTeamConflict)
            return Result<CreateMatchResponse>.Failure(ApplicationErrors.TeamConflict);

        var hasStadiumConflict = await matchRepository.ExistsStadiumConflictAsync(
            request.StadiumPublicId,
            request.StartTime,
            cancellationToken);

        if (hasStadiumConflict)
            return Result<CreateMatchResponse>.Failure(ApplicationErrors.StadiumConflict);

        var sameGroup = homeTeam.GroupPublicId.HasValue
            && homeTeam.GroupPublicId == awayTeam.GroupPublicId;

        if (sameGroup)
        {
            var existsSameMatch = await matchRepository.ExistsSameMatchAsync(
                request.HomeTeamPublicId,
                request.AwayTeamPublicId,
                cancellationToken);

            if (existsSameMatch)
                return Result<CreateMatchResponse>.Failure(ApplicationErrors.SameMatchExists);
        }

        var match = Domain.Entities.Match.Schedule(
            homeTeamPublicId: request.HomeTeamPublicId,
            awayTeamPublicId: request.AwayTeamPublicId,
            stadiumPublicId: request.StadiumPublicId,
            homeTeamName: homeTeam.TeamName,
            awayTeamName: awayTeam.TeamName,
            stadiumName: stadium.StadiumName,
            startTime: request.StartTime);

        await matchRepository.AddAsync(match, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CreateMatchResponse>.Success(new CreateMatchResponse
        {
            PublicId = match.PublicId
        });
    }
}
