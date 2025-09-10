namespace MMS.Domain.Repositories.User;

public interface IUserWriteOnlyRepository
{
    Task RegisterUser(Entities.User user);
}
