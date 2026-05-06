using Core;
using MediatR;

namespace Application.Knockout.RecordKnockoutResult;

public sealed class RecordKnockoutResultRequest : IRequest<Result<RecordKnockoutResultResponse>>
{
    public Guid? BracketPublicId { get; set; }
    public Guid? MatchPublicId { get; set; }
    public int? HomePoints { get; set; }
    public int? AwayPoints { get; set; }
}
