using MMS.Domain.Entities;

namespace MMS.Domain.Repositories.Company;

public interface ICompanyReadOnlyRepository
{
    IList<ShortCompany> ListShortCompanies();
    IList<ShortUser> ListUsers(long companyId);
}
