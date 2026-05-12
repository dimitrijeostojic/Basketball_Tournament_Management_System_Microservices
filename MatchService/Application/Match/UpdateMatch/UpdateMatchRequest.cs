using Core;
using MediatR;

namespace Application.Match.UpdateMatch;

public sealed class UpdateMatchRequest
    : IRequest<Result<UpdateMatchResponse>>
{
    public Guid MatchPublicId { get; set; }
    public Guid? HomeTeamPublicId { get; set; }
    public Guid? AwayTeamPublicId { get; set; }
    public Guid? StadiumPublicId { get; set; }
    public DateTime? StartTime { get; set; }
}
