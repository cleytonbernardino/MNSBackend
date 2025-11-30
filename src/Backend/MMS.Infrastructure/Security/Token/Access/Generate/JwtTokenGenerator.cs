using Microsoft.IdentityModel.Tokens;
using MMS.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MMS.Domain.Enums;
using MMS.Domain.Security.Token;

namespace MMS.Infrastructure.Security.Token.Access.Generate;

public class JwtTokenGenerator(
    uint expirationTimeMinutes,
    string signingKey,
    string issuer
    ) : JwtTokenHandle, IAccessTokenGenerator
{
    private readonly uint _expirationTimeMinutes = expirationTimeMinutes;
    private readonly string _signingKey = signingKey;
    private readonly string _issuer = issuer;

    public string Generate(Guid userIdentifier, UserRolesEnum role)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Sid, userIdentifier.ToString()),
            new Claim(ClaimTypes.Role, role.ToString())
        };

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
            SigningCredentials = new SigningCredentials(SecurityKey(_signingKey), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _issuer
        };

        JwtSecurityTokenHandler tokenHandle = new();
        SecurityToken securityToken = tokenHandle.CreateToken(tokenDescriptor);

        return tokenHandle.WriteToken(securityToken);
    }
    
    public string Generate(User user)
    {
        return Generate(user.UserIdentifier, user.Role);
    }
}
