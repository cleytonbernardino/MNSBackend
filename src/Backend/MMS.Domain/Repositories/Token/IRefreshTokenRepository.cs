using MMS.Domain.Entities;

namespace MMS.Domain.Repositories.Token;

public interface IRefreshTokenRepository
{
    Task SaveTokenAsync(RefreshToken token);
    Task<RefreshToken?> GetRefreshTokenWithUserIdentifier(string refreshToken, Guid userIdentifier);
}
