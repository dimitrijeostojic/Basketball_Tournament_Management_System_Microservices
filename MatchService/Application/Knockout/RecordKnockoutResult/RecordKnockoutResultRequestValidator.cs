using FluentValidation;

namespace Application.Knockout.RecordKnockoutResult;

public sealed class RecordKnockoutResultRequestValidator : AbstractValidator<RecordKnockoutResultRequest>
{
    public RecordKnockoutResultRequestValidator()
    {
        RuleFor(x => x.BracketPublicId)
            .NotNull()
            .NotEqual(Guid.Empty);

        RuleFor(x => x.MatchPublicId)
            .NotNull()
            .NotEqual(Guid.Empty);

        RuleFor(x => x.HomePoints)
            .NotNull()
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.AwayPoints)
            .NotNull()
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x)
            .Must(x => x.HomePoints != x.AwayPoints)
            .WithMessage("A knockout match cannot end in a draw.");
    }
}
