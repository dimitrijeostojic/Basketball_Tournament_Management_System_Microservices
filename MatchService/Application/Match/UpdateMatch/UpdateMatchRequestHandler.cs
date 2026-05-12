using Application.Common;
using Application.Contracts;
using Core;
using Domain.Abstraction;
using Domain.Enums;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Match.UpdateMatch;

internal sealed class UpdateMatchRequestHandler(
    IMatchRepository matchRepository,
    ITeamServiceClient teamServiceClient,
    IStadiumServiceClient stadiumServiceClient,
    IUnitOfWork unitOfWork
    )
    : IRequestHandler<UpdateMatchRequest, Result<UpdateMatchResponse>>
{
    private readonly IMatchRepository _matchRepository = matchRepository ?? throw new ArgumentNullException(nameof(matchRepository));
    private readonly ITeamServiceClient _teamServiceClient = teamServiceClient ?? throw new ArgumentNullException(nameof(teamServiceClient));
    private readonly IStadiumServiceClient _stadiumServiceClient = stadiumServiceClient ?? throw new ArgumentNullException(nameof(stadiumServiceClient));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<Result<UpdateMatchResponse>> Handle(UpdateMatchRequest request, CancellationToken cancellationToken)
    {
        var match = await _matchRepository.GetByPublicIdAsync(request.MatchPublicId, cancellationToken);
        if (match == null)
        {
            return Result<UpdateMatchResponse>.Failure(ApplicationErrors.NotFound);
        }

        if (match.Status == MatchStatus.Cancelled || match.Status == MatchStatus.Completed)
        {
            return Result<UpdateMatchResponse>.Failure(ApplicationErrors.InvalidOperation);
        }

        if (match.StartTime <= DateTime.UtcNow)
        {
            return Result<UpdateMatchResponse>.Failure(ApplicationErrors.InvalidOperation);
        }

        var homeTeam = (TeamValidationResponse?)null;
        var awayTeam = (TeamValidationResponse?)null;
        var stadium = (StadiumValidationResponse?)null;

        if (request.HomeTeamPublicId != null)
        {
            homeTeam = await _teamServiceClient.GetTeamByPublicIdAsync(request.HomeTeamPublicId.Value, cancellationToken);
            if (homeTeam is null || !homeTeam.Exists)
                return Result<UpdateMatchResponse>.Failure(ApplicationErrors.NotFound);
        }
        if (request.AwayTeamPublicId != null)
        {
            awayTeam = await _teamServiceClient.GetTeamByPublicIdAsync(request.AwayTeamPublicId.Value, cancellationToken);
            if (awayTeam is null || !awayTeam.Exists)
                return Result<UpdateMatchResponse>.Failure(ApplicationErrors.NotFound);
        }
        if (request.StadiumPublicId != null)
        {
            stadium = await _stadiumServiceClient.GetStadiumByPublicIdAsync(request.StadiumPublicId.Value, cancellationToken);
            if (stadium is null || !stadium.Exists)
                return Result<UpdateMatchResponse>.Failure(ApplicationErrors.NotFound);
        }

        match = match.UpdateHomeTeam(homeTeam?.PublicId, homeTeam?.TeamName)
                    .UpdateAwayTeam(awayTeam?.PublicId, awayTeam?.TeamName)
                    .UpdateStadium(stadium?.PublicId, stadium?.StadiumName)
                    .UpdateStartTime(request.StartTime);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<UpdateMatchResponse>.Success(new UpdateMatchResponse { Match = match });
    }
}
