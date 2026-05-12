using FluentValidation;

namespace Application.Match.DeleteMatch;

public sealed class DeleteMatchValidator
    : AbstractValidator<DeleteMatchRequest>
{
    public DeleteMatchValidator()
    {
        RuleFor(x => x.MatchPublicId)
            .NotEmpty().WithMessage("Match public ID is required.");
    }
}
