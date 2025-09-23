using Entity = MMS.Domain.Entities;

namespace MMS.Domain.Repositories.Company;

public interface ICompanyWriteOnlyRepository
{
    Task RegisterCompany(Entity.Company company);
    Task Delete(long id);
}
