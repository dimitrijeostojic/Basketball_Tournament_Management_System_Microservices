using Core;
using MediatR;

namespace Application.Match.GetMatchByPublicId;

public sealed class GetMatchByPublicIdRequest
    : IRequest<Result<GetMatchByPublicIdResponse>>
{
    public Guid MatchPublicId { get; set; }
    public Guid StadiumPublicId { get; set; }
    public DateTime ScheduledDateTime { get; set; }
}
