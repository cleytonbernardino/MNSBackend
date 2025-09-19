using MMS.Communication.Requests.Auth;
using MMS.Communication.Responses.Auth;
using MMS.Domain.Enums;
using MMS.Domain.Security.Token;
using MMS.Domain.ValueObjects;
using MMS.Exceptions.ExceptionsBase;

namespace MMS.Application.UseCases.Auth.RefreshToken;

public class RefreshTokenUseCase(
    IRefreshTokenHandler refreshTokenHandler,
    IAccessTokenGenerator accessTokenGenerator
) : IRefreshTokenUseCase
{
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;
    private readonly IRefreshTokenHandler _refreshTokenHandler = refreshTokenHandler;

    private string? _refreshToken;

    public async Task<ResponseRefreshToken> Execute(RequestRefreshToken request, string refreshToken)
    {
        await Validate(request);

        (Guid userIdentifier, string role) accessTokenData =
            _refreshTokenHandler.ValidateAccessTokenAndGetData(request.AccessToken);

        var refreshTokenValidated = await _refreshTokenHandler.GetRefreshToken(refreshToken, accessTokenData.userIdentifier);
        if (refreshTokenValidated is null)
            throw new NoPermissionException();

        if (refreshTokenValidated.ExpiryDate < DateTime.UtcNow.AddDays(MMSConst.SECURITY_DAYS_TO_REMAKE_TOKEN))
            refreshTokenValidated.Token =
                await _refreshTokenHandler.GenerateTokenAndSave(accessTokenData.userIdentifier);
        _refreshToken = refreshTokenValidated.Token;

        return new ResponseRefreshToken
        {
            AccessToken = _accessTokenGenerator.Generate(
                accessTokenData.userIdentifier, Enum.Parse<UserRolesEnum>(accessTokenData.role))
        };
    }

    public string? GetRefreshToken() => _refreshToken;

    private static async Task Validate(RequestRefreshToken request)
    {
        RefreshTokenValidator validator = new();
        var result = await validator.ValidateAsync(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(
                result.Errors.Select(e => e.ErrorMessage).ToArray());
    }
}
