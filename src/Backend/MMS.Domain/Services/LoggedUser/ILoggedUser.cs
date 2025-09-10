using MMS.Domain.Entities;

namespace MMS.Domain.Services.LoggedUser;

public interface ILoggedUser
{
    Task<User> User();
}
