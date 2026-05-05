using Application.Common;
using Core;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.TwoFactor.Confirm;

public sealed class ConfirmTwoFactorRequestHandler(
    UserManager<User> userManager
    )
    : IRequestHandler<ConfirmTwoFactorRequest, Result<ConfirmTwoFactorResponse>>
{
    private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

    public async Task<Result<ConfirmTwoFactorResponse>> Handle(ConfirmTwoFactorRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
            return Result<ConfirmTwoFactorResponse>.Failure(ApplicationErrors.NotFound);

        var isValid = await _userManager.VerifyTwoFactorTokenAsync(
            user,
            _userManager.Options.Tokens.AuthenticatorTokenProvider,
            request.Code);

        if (!isValid)
            return Result<ConfirmTwoFactorResponse>.Failure(ApplicationErrors.InvalidTwoFactorCode);

        await _userManager.SetTwoFactorEnabledAsync(user, true);

        return Result<ConfirmTwoFactorResponse>.Success(
            new ConfirmTwoFactorResponse("Two-factor authentication has been enabled."));
    }
}
