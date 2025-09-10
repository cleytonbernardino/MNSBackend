using MMS.Communication;
using MMS.Domain.Entities;
using MMS.Domain.Enums;

namespace MMS.Application.Extensions;

public static class ProtoUserToEntityUser
{
    public static User Join(this User user, RequestUpdateUser request)
    {
        if (!string.IsNullOrEmpty(request.Email))
            user.Email = request.Email;

        if (!string.IsNullOrEmpty(request.LastName))
            user.LastName = request.LastName;

        if (!string.IsNullOrEmpty(request.Phone))
            user.Phone = request.Phone;

        if (request.Role != 0)
            user.Role = (UserRolesEnum)request.Role;

        return user;
    }

    public static ResponseShortUser ToShortResponse(this ShortUser user)
    {
        return new ResponseShortUser
        {
            FirstName = user.FirstName,
            LastName = user.LastName ?? "",
            LastLogin = user.LastLogin.ToString() ?? "",
            Role = user.Role.ToString(),
        };
    }
}
