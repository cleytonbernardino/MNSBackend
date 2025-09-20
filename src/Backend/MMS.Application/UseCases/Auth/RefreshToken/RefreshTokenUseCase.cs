using MMS.Communication.Responses.Auth;
using MMS.Domain.Enums;
using MMS.Domain.Security.Token;
using MMS.Domain.ValueObjects;
using MMS.Exceptions;
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

    public async Task<ResponseRefreshToken> Execute(string refreshToken, string accessToken)
    {
        (Guid userIdentifier, string role) accessTokenData =
            _refreshTokenHandler.ValidateAccessTokenAndGetData(accessToken);

        var refreshTokenValidated = await _refreshTokenHandler.GetRefreshToken(refreshToken, accessTokenData.userIdentifier);
        if (refreshTokenValidated is null)
            throw new NoPermissionException(ResourceMessagesException.REFRESH_TOKEN_NOT_FOUND);

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
}
