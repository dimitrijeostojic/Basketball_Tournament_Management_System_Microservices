using Core;
using MediatR;

namespace Application.Knockout.CreateKnockoutBracket;

public sealed class CreateKnockoutBracketRequest : IRequest<Result<CreateKnockoutBracketResponse>>
{
    // Poredjani po seedu: index 0 = seed 1 (najbolji), index 7 = seed 8 (najlošiji)
    public List<Guid> SeededTeamPublicIds { get; set; } = [];
}
