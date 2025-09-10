using MMS.Domain.Entities;
using MMS.Domain.Enums;
using MMS.Domain.Services.Authorization;

namespace MMS.Infrastructure.Services.Authorization;

public class AuthorizationService : IAuthorizationServices
{
    private static bool HasSpecialPermission(User user)
    {
        return user.IsAdmin ||
            user.Role == UserRolesEnum.MANAGER ||
            user.Role == UserRolesEnum.SUB_MANAGER ||
            user.Role == UserRolesEnum.RH;
    }

    public bool CanCreateUser(User user) => HasSpecialPermission(user);

    public bool CanDeleteUser(User user) => HasSpecialPermission(user);

    public bool CanReadUsers(User user) => HasSpecialPermission(user);

    public bool CanUpdateUser(User user) => HasSpecialPermission(user);

    public bool CanUpdateUserRole(User user)
    {
        if (user.Role == UserRolesEnum.RH)
            return false;

        return HasSpecialPermission(user);
    }
}
