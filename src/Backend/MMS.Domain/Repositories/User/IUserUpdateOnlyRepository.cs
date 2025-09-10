namespace MMS.Domain.Repositories.User;

public interface IUserUpdateOnlyRepository
{
    Task<Entities.User?> GetById(long id, long companyId);
    Task<Entities.User?> GetUserByEmailAndPassword(string email, string password);
    void UpdateUser(Entities.User user);
}
