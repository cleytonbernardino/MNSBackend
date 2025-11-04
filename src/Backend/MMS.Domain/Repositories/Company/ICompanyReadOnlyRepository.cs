using Entity = MMS.Domain.Entities;

namespace MMS.Domain.Repositories.Company;

public interface ICompanyReadOnlyRepository
{
    Entity.ShortCompany[] ListShortCompanies();
    Entity.ShortUser[] ListUsers(long companyId);
    Task<Entity.Company?> GetById(Entity.User user, long companyId);
    Task<bool> Exists(long companyId);
}
