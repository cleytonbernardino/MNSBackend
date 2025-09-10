using Microsoft.IdentityModel.Tokens;

namespace MMS.Infrastructure.Security.Token.Access.Validator;

public interface IAccessTokenValidator
{
    public TokenValidationParameters GetParameters();
}
