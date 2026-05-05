using Core;
using MediatR;

namespace Application.TwoFactor.Confirm;

public sealed class ConfirmTwoFactorRequest : IRequest<Result<ConfirmTwoFactorResponse>>
{
    public string UserId { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
