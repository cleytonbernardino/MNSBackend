using Entity = MMS.Domain.Entities;

namespace MMS.Domain.Repositories.Company;

public interface ICompanyReadOnlyRepository
{
    IList<Entity.ShortCompany> ListShortCompanies();
    Entity.ShortUser[] ListUsers(long companyId);
    Task<Entity.Company?> GetById(Entity.User user, long companyId);
}
