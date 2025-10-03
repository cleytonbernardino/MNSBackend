using Entity = MMS.Domain.Entities;

namespace MMS.Domain.Repositories.Company;

public interface ICompanyUpdateOnlyRepository
{
    Task<Entity.Company?> GetById(long id);
    void Update(Entity.Company company);
}
