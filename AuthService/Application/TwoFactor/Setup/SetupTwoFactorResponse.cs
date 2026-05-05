namespace Application.TwoFactor.Setup;

public sealed record SetupTwoFactorResponse(
    string ManualEntryKey,
    string QrCodeBase64
);
