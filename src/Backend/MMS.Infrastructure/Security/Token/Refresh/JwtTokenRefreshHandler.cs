using MMS.Domain.Entities;
using MMS.Domain.Repositories;
using MMS.Domain.Repositories.Token;
using MMS.Domain.Security.Token;
using MMS.Exceptions.ExceptionsBase;
using MMS.Infrastructure.Security.Token.Access.Validator;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace MMS.Infrastructure.Security.Token.Refresh;

public class JwtTokenRefreshHandler(
    uint expiryDate,
    IAccessTokenValidator accessTokenValidator,
    IRefreshTokenRepository refreshTokenRepository,
    IUnitOfWork unitOfWork
    ) : IRefreshTokenHandler
{
    private readonly uint _expiryDate = expiryDate;
    private readonly IAccessTokenValidator _accessTokenValidator = accessTokenValidator;
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    private static string Generate()
    {
        byte[] randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<string> GenerateTokenAndSave(Guid userIdentifier)
    {
        RefreshToken token = new()
        {
            Token = Generate(),
            ExpiryDate = DateTime.UtcNow.AddDays(_expiryDate),
            UserIdentifier = userIdentifier
        };
        await _refreshTokenRepository.SaveTokenAsync(token);
        await _unitOfWork.Commit();
        
        return token.Token;
    }
    
    public (Guid userIdentifier, string role) ValidateAccessTokenAndGetData(string token)
    {
        var validationParaments = _accessTokenValidator.GetParameters();

        JwtSecurityTokenHandler tokenHandler = new();
        var principal = tokenHandler.ValidateToken(token, validationParaments, out _);

        string? userIdentifierValue = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

        if (!Guid.TryParse(userIdentifierValue, out Guid userIdentifier))
            throw new NoPermissionException();
        
        string role = principal.Claims.First(c => c.Type == ClaimTypes.Role).Value;

        return (userIdentifier, role);
    }

    public async Task<RefreshToken?> GetRefreshToken(string token, Guid userIdentifier) =>
        await _refreshTokenRepository.GetRefreshTokenWithUserIdentifier(token, userIdentifier);

    public async Task<bool> Delete(string refreshToken, Guid userIdentifier)
    {
        var rowsAffect =  await _refreshTokenRepository.Delete(refreshToken, userIdentifier);
        await _unitOfWork.Commit();
        return rowsAffect;
    }
}
