using FluentValidation;

namespace Application.Match.UpdateMatch;

public sealed class UpdateMatchValidator
    : AbstractValidator<UpdateMatchRequest>
{
    public UpdateMatchValidator()
    {
        RuleFor(x => x.MatchPublicId)
            .NotEmpty().WithMessage("Match public ID is required.");

        RuleFor(x => x.HomeTeamPublicId)
            .NotEqual(Guid.Empty)
            .When(x => x.HomeTeamPublicId.HasValue);

        RuleFor(x => x.AwayTeamPublicId)
            .NotEqual(Guid.Empty)
            .When(x => x.AwayTeamPublicId.HasValue);

        RuleFor(x => x.StadiumPublicId)
            .NotEqual(Guid.Empty)
            .When(x => x.StadiumPublicId.HasValue);

        RuleFor(x => x.StartTime)
            .GreaterThan(DateTime.UtcNow).WithMessage("StartTime must be in the future.")
            .When(x => x.StartTime.HasValue);

        RuleFor(x => x)
            .Must(x => x.HomeTeamPublicId != x.AwayTeamPublicId)
            .WithMessage("Home and away team must be different.")
            .When(x => x.HomeTeamPublicId.HasValue && x.AwayTeamPublicId.HasValue);
    }
}
