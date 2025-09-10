using MMS.Communication;
using MMS.Domain.Enums;
using MMS.Domain.Security.Token;
using MMS.Domain.ValueObjects;
using MMS.Exceptions.ExceptionsBase;

namespace MMS.Application.UseCases.Login.RefreshToken;

public class RefreshTokenUseCase(
    IRefreshTokenHandler refreshTokenHandler,
    IAccessTokenGenerator accessTokenGenerator
    ) : IRefreshTokenUseCase
{
    private readonly IRefreshTokenHandler _refreshTokenHandler = refreshTokenHandler;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;
    
    public async Task<ResponseToken> Execute(RequestRefreshToken request)
    {
        await Validate(request);
        
        var accessTokenData = _refreshTokenHandler.GetDataFromAccessToken(request.AccessToken);

        var refreshToken = await _refreshTokenHandler.GetToken(request.RefreshToken, accessTokenData.userIdentifier);
        if (refreshToken is null)
            throw new NoPermissionException();
        
        if(refreshToken.ExpiryDate < DateTime.UtcNow.AddDays(MMSConst.SECURITY_DAYS_TO_REMAKE_TOKEN))
            refreshToken.Token = await _refreshTokenHandler.GenerateTokenAndSave(accessTokenData.userIdentifier);
        
        return new ResponseToken
        {
            AccessToken = _accessTokenGenerator.Generate(
                accessTokenData.userIdentifier, Enum.Parse<UserRolesEnum>(accessTokenData.role)),
            RefreshToken = refreshToken.Token
        };
    }

    private static async Task Validate(RequestRefreshToken request)
    {
        RefreshTokenValidator validator = new();
        var result = await validator.ValidateAsync(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(
                result.Errors.Select(e => e.ErrorMessage).ToArray());
    }
}
