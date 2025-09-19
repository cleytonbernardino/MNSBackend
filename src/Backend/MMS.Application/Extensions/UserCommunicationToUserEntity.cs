using MMS.Communication.Requests.User;
using MMS.Communication.Responses.User;
using MMS.Domain.Entities;
using MMS.Domain.Enums;

namespace MMS.Application.Extensions;

internal static class UserCommunicationToUserEntity
{
    public static User ToUser(this RequestRegisterUser request)
    {
        return new User
        {
            UpdatedOn = DateTime.UtcNow,
            Email = request.Email,
            Phone = request.Phone.Replace(" ", "").Replace("-", ""),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = (UserRolesEnum)request.Role
        };
    }

    public static User Join(this RequestUpdateUser request, User user)
    {
        user.UpdatedOn = DateTime.UtcNow;
        if (!string.IsNullOrWhiteSpace(request.Email))
            user.Email = request.Email;

        if (!string.IsNullOrWhiteSpace(request.Phone))
            user.Phone = request.Phone.Replace(" ", "").Replace("-", "");

        if (!string.IsNullOrWhiteSpace(request.LastName))
            user.LastName = request.LastName;

        if (Enum.IsDefined(typeof(UserRolesEnum), request.Role))
            user.Role = (UserRolesEnum)request.Role;

        return user;
    }

    public static ResponseShortUser ToShortResponse(this ShortUser user)
    {
        return new ResponseShortUser
        {
            FirstName = user.FirstName,
            LastName = user.LastName ?? "",
            LastLogin = user.LastLogin,
            Role = user.Role.ToString()
        };
    }
}
