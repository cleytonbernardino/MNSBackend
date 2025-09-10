using MMS.Domain.Entities;

namespace MMS.Domain.Security.Token;

public interface IRefreshTokenHandler
{
    Task<string> GenerateTokenAndSave(Guid userIdentifier);
    (Guid userIdentifier, string role, bool isAdmin) GetDataFromAccessToken(string token);
    Task<RefreshToken?> GetToken(string token, Guid userIdentifier);
}
