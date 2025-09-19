using Microsoft.Extensions.Logging;
using MMS.Communication.Requests.Auth;
using MMS.Domain.Security.Token;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;

namespace MMS.Application.UseCases.Auth.Logout;

public class LogoutUseCase(
    IRefreshTokenHandler tokenHandler
) : ILogoutUseCase
{
    private readonly IRefreshTokenHandler _tokenHandler = tokenHandler;

    public async Task Execute(RequestRefreshToken request, string refreshToken)
    {
        (Guid userIdentifier, string role) accessTokenData =
            _tokenHandler.ValidateAccessTokenAndGetData(request.AccessToken);
        bool status = await _tokenHandler.Delete(refreshToken, accessTokenData.userIdentifier);

        if (!status)
        {
            throw new ErrorOnValidationException([
                ResourceMessagesException.INVALID_ACCESS_TOKEN, ResourceMessagesException.INVALID_REFRESH_TOKEN
            ]);
        }
    }
}
