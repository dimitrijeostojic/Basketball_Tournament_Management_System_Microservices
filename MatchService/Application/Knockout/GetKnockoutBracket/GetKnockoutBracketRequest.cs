using Core;
using MediatR;

namespace Application.Knockout.GetKnockoutBracket;

public sealed class GetKnockoutBracketRequest : IRequest<Result<GetKnockoutBracketResponse>>
{
    public Guid? BracketPublicId { get; set; }
}
