using Microsoft.EntityFrameworkCore;
using MMS.Domain.Entities;
using MMS.Domain.Repositories.Company;

namespace MMS.Infrastructure.DataAccess.Repositories;

public class CompanyRepository(
    MmsDbContext dbContext
) : ICompanyWriteOnlyRepository, ICompanyReadOnlyRepository
{
    private readonly MmsDbContext _dbContext = dbContext;

    public IList<ShortCompany> ListShortCompanies()
    {
        return _dbContext
            .Companies
            .AsNoTracking()
            .Select(field => new ShortCompany
            {
                Id = field.Id,
                DoingBusinessAs = field.LegalName,
                SubscriptionStatus = field.SubscriptionStatus,
                SubscriptionPlan =
                    field.CompanySubscription != null
                        ? field.CompanySubscription.SubscriptionPlan.Name
                        : string.Empty,
                ManagerName = field.Manager != null ? field.Manager.FirstName : string.Empty
            }).ToList();
    }

    public IList<ShortUser> ListUsers(long companyId)
    {
        return _dbContext.Users
            .AsNoTracking()
            .Where(user => user.CompanyId == companyId && user.Active)
            .Select(field => new ShortUser
            {
                Id = field.Id,
                FirstName = field.FirstName,
                LastName = field.LastName,
                Role = field.Role,
                LastLogin = field.LastLogin
            })
            .ToList();
    }

    public async Task RegisterCompany(Company company)
    {
        await _dbContext
            .Companies
            .AddAsync(company);
    }
}
