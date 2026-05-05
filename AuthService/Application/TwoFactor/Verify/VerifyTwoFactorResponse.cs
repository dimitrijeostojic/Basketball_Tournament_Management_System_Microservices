namespace Application.TwoFactor.Verify;

public sealed record VerifyTwoFactorResponse(string AccessToken, string RefreshToken);
