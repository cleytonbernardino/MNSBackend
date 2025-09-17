using MMS.Domain.Entities;

namespace MMS.Domain.Security.Token;

public interface IRefreshTokenHandler
{
    Task<string> GenerateTokenAndSave(Guid userIdentifier);
    (Guid userIdentifier, string role) ValidateAccessTokenAndGetData(string token);
    Task<RefreshToken?> GetToken(string token, Guid userIdentifier);
    Task<int> Delete(string refreshToken, Guid userIdentifier);
}
