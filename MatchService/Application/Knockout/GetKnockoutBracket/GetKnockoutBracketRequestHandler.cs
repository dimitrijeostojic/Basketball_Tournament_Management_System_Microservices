using Application.Common;
using Core;
using Domain.RepositoryInterfaces;
using MediatR;

namespace Application.Knockout.GetKnockoutBracket;

public sealed class GetKnockoutBracketRequestHandler(IKnockoutBracketRepository bracketRepository)
    : IRequestHandler<GetKnockoutBracketRequest, Result<GetKnockoutBracketResponse>>
{
    private readonly IKnockoutBracketRepository _bracketRepository = bracketRepository ?? throw new ArgumentNullException(nameof(bracketRepository));

    public async Task<Result<GetKnockoutBracketResponse>> Handle(
        GetKnockoutBracketRequest request,
        CancellationToken cancellationToken)
    {
        var bracket = await _bracketRepository.GetByPublicIdAsync(request.BracketPublicId!.Value, cancellationToken);

        if (bracket is null)
            return Result<GetKnockoutBracketResponse>.Failure(ApplicationErrors.NotFound);

        var matchDtos = bracket.Matches
            .OrderBy(m => m.Round)
            .ThenBy(m => m.MatchOrder)
            .Select(m => new KnockoutMatchDto
            {
                PublicId = m.PublicId,
                Round = m.Round,
                MatchOrder = m.MatchOrder,
                HomeTeamPublicId = m.HomeTeamPublicId,
                HomeTeamName = m.HomeTeamName,
                AwayTeamPublicId = m.AwayTeamPublicId,
                AwayTeamName = m.AwayTeamName,
                HomePoints = m.HomePoints,
                AwayPoints = m.AwayPoints,
                WinnerPublicId = m.WinnerPublicId,
                Status = m.Status
            })
            .ToList();

        return Result<GetKnockoutBracketResponse>.Success(new GetKnockoutBracketResponse(matchDtos));
    }
}
