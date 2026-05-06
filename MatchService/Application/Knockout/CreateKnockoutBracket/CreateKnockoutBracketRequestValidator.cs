using FluentValidation;

namespace Application.Knockout.CreateKnockoutBracket;

public sealed class CreateKnockoutBracketRequestValidator : AbstractValidator<CreateKnockoutBracketRequest>
{
    public CreateKnockoutBracketRequestValidator()
    {
        RuleFor(x => x.SeededTeamPublicIds)
            .NotEmpty()
            .Must(ids => ids.Count == 8)
            .WithMessage("Exactly 8 seeded teams are required.")
            .Must(ids => ids.Distinct().Count() == ids.Count)
            .WithMessage("Seeded teams must all be different.")
            .Must(ids => ids.All(id => id != Guid.Empty))
            .WithMessage("Team IDs must not be empty GUIDs.");
    }
}
