using Microsoft.Extensions.Logging;
using MMS.Communication;
using MMS.Domain.Security.Token;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;

namespace MMS.Application.UseCases.Auth.Logout;

public class LogoutUseCase(
    IRefreshTokenHandler tokenHandler,
    ILogger logger
    ): ILogoutUseCase
{
    private readonly IRefreshTokenHandler _tokenHandler = tokenHandler;
    private readonly ILogger _logger = logger;
    
    public async Task Execute(RequestRefreshAccessToken request, string refreshToken)
    {
        var accessTokenData=  _tokenHandler.ValidateAccessTokenAndGetData(request.AccessToken);
        var status = await  _tokenHandler.Delete(refreshToken, accessTokenData.userIdentifier);

        switch (status)
        {
            case 0:
                throw new ErrorOnValidationException([
                    ResourceMessagesException.INVALID_ACCESS_TOKEN, ResourceMessagesException.INVALID_REFRESH_TOKEN
                ]);
            case > 1:
                _logger.LogCritical("Mais de um refresh token foi apagado, na ultima request");
                break;
        }
    }
}
