using MMS.Domain.Security.Token;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;

namespace MMS.Application.UseCases.Auth.Logout;

public class LogoutUseCase(
    IRefreshTokenHandler tokenHandler
) : ILogoutUseCase
{
    private readonly IRefreshTokenHandler _tokenHandler = tokenHandler;

    public async Task Execute(string refreshToken, string accessToken)
    {
        (Guid userIdentifier, string role) accessTokenData =
            _tokenHandler.ValidateAccessTokenAndGetData(accessToken);
        bool status = await _tokenHandler.Delete(refreshToken, accessTokenData.userIdentifier);

        if (!status)
        {
            throw new ErrorOnValidationException([
                ResourceMessagesException.INVALID_ACCESS_TOKEN, ResourceMessagesException.INVALID_REFRESH_TOKEN
            ]);
        }
    }
}
