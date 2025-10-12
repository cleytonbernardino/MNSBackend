using Microsoft.EntityFrameworkCore;
using MMS.Domain.Entities;
using MMS.Domain.Repositories.Company;

namespace MMS.Infrastructure.DataAccess.Repositories;

public class CompanyRepository(
    MmsDbContext dbContext
) : ICompanyWriteOnlyRepository, ICompanyReadOnlyRepository, ICompanyUpdateOnlyRepository
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

    public ShortUser[] ListUsers(long companyId)
    {
        return _dbContext.Users
            .AsNoTracking()
            .Where(user => user.CompanyId == companyId)
            .Select(field => new ShortUser
            {
                Id = field.Id,
                FirstName = field.FirstName,
                Email = field.Email,
                Role = field.Role,
                Active = field.Active
            })
            .ToArray();
    }

    public async Task<Company?> GetById(User user, long companyId)
    {
        var query = _dbContext.Companies
            .AsNoTracking()
            .Include(company => company.Manager)
            .Where(company => company.Id == companyId);
        if (user.IsAdmin)
            return await query.FirstOrDefaultAsync();
        
        query = query.Where(company => company.ManagerId == user.Id && company.Active);
        return await query.FirstOrDefaultAsync();
    }

    public async Task RegisterCompany(Company company)
    {
        await _dbContext
            .Companies
            .AddAsync(company);
    }

    public async Task Delete(long id)
    {
        var company = await _dbContext.Companies.FirstOrDefaultAsync(company => company.Id == id);
        if (company is null)
            return;

        _dbContext.Companies.Remove(company);
    }

    public async Task<Company?> GetById(long id)
    {
        return await _dbContext.Companies.FirstOrDefaultAsync(company => company.Id == id);
    }

    public void Update(Company company) => _dbContext.Companies.Update(company);
}
