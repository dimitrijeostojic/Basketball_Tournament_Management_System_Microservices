using Core;
using MediatR;

namespace Application.Knockout.ScheduleKnockoutMatch;

public sealed class ScheduleKnockoutMatchRequest
    : IRequest<Result<ScheduleKnockoutMatchResponse>>
{
    public Guid MatchPublicId { get; set; }
    public DateTime ScheduledDateTime { get; set; }
    public Guid? StadiumPublicId { get; set; }
}
