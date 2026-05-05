using FluentValidation;

namespace Application.TwoFactor.Confirm;

public sealed class ConfirmTwoFactorRequestValidator : AbstractValidator<ConfirmTwoFactorRequest>
{
    public ConfirmTwoFactorRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty().Length(6);
    }
}
