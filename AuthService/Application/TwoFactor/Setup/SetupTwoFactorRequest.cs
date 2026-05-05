using Core;
using MediatR;

namespace Application.TwoFactor.Setup;

public sealed class SetupTwoFactorRequest : IRequest<Result<SetupTwoFactorResponse>>
{
    public string UserId { get; set; } = string.Empty;
}
