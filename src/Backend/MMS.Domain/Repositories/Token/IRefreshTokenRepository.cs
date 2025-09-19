using MMS.Domain.Entities;

namespace MMS.Domain.Repositories.Token;

public interface IRefreshTokenRepository
{
    Task SaveTokenAsync(RefreshToken token);
    Task<RefreshToken?> GetRefreshTokenWithUserIdentifier(string refreshToken, Guid userIdentifier);
    Task<bool> Delete(string refreshToken, Guid userIdentifier);
}
