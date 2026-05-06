using Core;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Match.GetMatches;

public sealed class GetMatchesRequestHandler(IMatchRepository matchRepository)
    : IRequestHandler<GetMatchesRequest, Result<GetMatchesResponse>>
{
    public async Task<Result<GetMatchesResponse>> Handle(
        GetMatchesRequest request,
        CancellationToken cancellationToken)
    {
        var matches = await matchRepository.GetAllAsync(cancellationToken);

        var dtos = matches.Select(m => new GetMatchesDto
        {
            PublicId = m.PublicId,
            HomeTeam = m.HomeTeamName,
            AwayTeam = m.AwayTeamName,
            Stadium = m.StadiumName,
            StartTime = m.StartTime,
            HomePoints = m.HomePoints,
            AwayPoints = m.AwayPoints,
            Status = m.Status
        }).ToList();

        return Result<GetMatchesResponse>.Success(new GetMatchesResponse(dtos));
    }
}
