using MMS.Communication;
using MMS.Domain.Entities;
using MMS.Domain.Enums;

namespace MMS.Application.Extensions;

internal static class UserRegisterToUser
{
    public static User ToUser(this RequestRegisterUser request)
    {
        return new User()
        {
            Email = request.Email,
            Phone = request.Phone.Replace(" ", "").Replace("-", ""),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = (UserRolesEnum)request.Role,
        };
    }
}
