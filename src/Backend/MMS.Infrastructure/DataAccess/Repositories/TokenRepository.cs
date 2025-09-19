using Microsoft.EntityFrameworkCore;
using MMS.Domain.Entities;
using MMS.Domain.Repositories.Token;

namespace MMS.Infrastructure.DataAccess.Repositories;

public class TokenRepository(
    MmsDbContext dbContext
) : IRefreshTokenRepository
{
    private MmsDbContext _DbContext => dbContext;

    public async Task SaveTokenAsync(RefreshToken token)
    {
        await _DbContext.RefreshTokens.AddAsync(token);
    }

    public async Task<RefreshToken?> GetRefreshTokenWithUserIdentifier(string refreshToken, Guid userIdentifier)
    {
        return await _DbContext.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(token =>
                token.UserIdentifier == userIdentifier && token.Token == refreshToken
            );
    }

    public async Task<bool> Delete(string refreshToken, Guid userIdentifier)
    {
        var result =  await _DbContext.RefreshTokens
            .FirstOrDefaultAsync(repository =>
                repository.Token == refreshToken
                && repository.UserIdentifier == userIdentifier);
        if (result is null)
        {
            return false;
        }
        _DbContext.RefreshTokens.Remove(result);
        return true;
    }
}
