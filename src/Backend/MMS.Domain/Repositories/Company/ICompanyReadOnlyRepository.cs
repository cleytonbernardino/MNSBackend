using MMS.Domain.Entities;

namespace MMS.Domain.Repositories.Company;

public interface ICompanyReadOnlyRepository
{
    IList<ShortCompany> ListCompanies();
    IList<ShortUser> ListUsers(long companyId);
}
