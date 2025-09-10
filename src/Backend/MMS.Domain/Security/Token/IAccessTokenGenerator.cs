using MMS.Domain.Enums;

namespace MMS.Domain.Security.Token;

public interface IAccessTokenGenerator
{
    string Generate(Guid userIdentifier, UserRolesEnum role);
}
