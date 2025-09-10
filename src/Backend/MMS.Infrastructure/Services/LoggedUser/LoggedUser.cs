using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MMS.Domain.Entities;
using MMS.Domain.Security.Token;
using MMS.Domain.Services.LoggedUser;
using MMS.Infrastructure.DataAccess;

namespace MMS.Infrastructure.Services.LoggedUser;

public class LoggedUser(
    MmsDbContext dbContext,
    ITokenProvider tokenProvider
    ) : ILoggedUser
{
    private readonly MmsDbContext _dbContext = dbContext;
    private readonly ITokenProvider _tokenProvider = tokenProvider;

    public async Task<User> User()
    {
        string token = _tokenProvider.Value().Trim();

        JwtSecurityTokenHandler tokenHandle = new();

        var jwtSecurityToken = tokenHandle.ReadJwtToken(token);

        string identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

        Guid userIdentifier = Guid.Parse(identifier);

        return await _dbContext
            .Users
            .AsNoTracking()
            .FirstAsync(user => user.UserIdentifier == userIdentifier && user.Active);
    }
}
