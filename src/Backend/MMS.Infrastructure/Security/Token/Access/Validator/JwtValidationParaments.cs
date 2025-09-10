using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MMS.Infrastructure.Security.Token.Access.Validator;

public class JwtValidationParaments(
    string signingKey,
    string issueKey
    ) : IAccessTokenValidator
{
    private readonly string _signingKey = signingKey;
    private readonly string _issueKey = issueKey;

    public static TokenValidationParameters ConfigureParameters(string signingKey, string issueKey)
    { 
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
            ClockSkew = new TimeSpan(0),
            ValidateIssuer = true,
            ValidIssuer = issueKey,
            ValidateLifetime = true,
            ValidateAudience = false,
            ValidAlgorithms = [SecurityAlgorithms.HmacSha256]
        };
    }
    
    public TokenValidationParameters GetParameters()
    {
        return ConfigureParameters(_signingKey, _issueKey);
    }
}
