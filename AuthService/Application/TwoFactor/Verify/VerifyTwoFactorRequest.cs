using Core;
using MediatR;

namespace Application.TwoFactor.Verify;

public sealed class VerifyTwoFactorRequest : IRequest<Result<VerifyTwoFactorResponse>>
{
    public string UserId { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
