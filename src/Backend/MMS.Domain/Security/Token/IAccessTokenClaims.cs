namespace MMS.Domain.Security.Token;

public interface IAccessTokenClaims
{
    Guid GetUserIdentifier();
}
