using Application.Common;
using Core;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using QRCoder;

namespace Application.TwoFactor.Setup;

public sealed class SetupTwoFactorRequestHandler(
    UserManager<User> userManager
    )
    : IRequestHandler<SetupTwoFactorRequest, Result<SetupTwoFactorResponse>>
{
    private const string Issuer = "SPQatar2027";

    private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

    public async Task<Result<SetupTwoFactorResponse>> Handle(SetupTwoFactorRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
            return Result<SetupTwoFactorResponse>.Failure(ApplicationErrors.NotFound);

        // Generate a new authenticator key and store it in AspNetUserTokens
        await _userManager.ResetAuthenticatorKeyAsync(user);
        var key = await _userManager.GetAuthenticatorKeyAsync(user);

        // Format: groups of 4 for manual entry readability
        var manualEntryKey = string.Join(" ", Enumerable
            .Range(0, key!.Length / 4)
            .Select(i => key.Substring(i * 4, 4)));

        // Build otpauth URI for authenticator apps
        var otpUri = $"otpauth://totp/{Uri.EscapeDataString(Issuer)}:{Uri.EscapeDataString(user.Email!)}?secret={key}&issuer={Uri.EscapeDataString(Issuer)}&digits=6&period=30";

        // Generate QR code as base64 PNG
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(otpUri, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeData);
        var qrBytes = qrCode.GetGraphic(10);
        var qrBase64 = Convert.ToBase64String(qrBytes);

        return Result<SetupTwoFactorResponse>.Success(
            new SetupTwoFactorResponse(manualEntryKey, qrBase64));
    }
}
