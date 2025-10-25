using Microsoft.EntityFrameworkCore;
using MMS.Domain.Entities;
using MMS.Domain.Repositories.CompanySubscription;

namespace MMS.Infrastructure.DataAccess.Repositories;

internal class SubscriptionsRepository(
    MmsDbContext dbContext
    ) : ICompanySubscriptionWriteOnlyRepository
{
    #region Dependency Injection

    private readonly MmsDbContext _dbContext = dbContext;

    #endregion

    private async Task ChangePlanAsync(CompanySubscription companySubscription)
    {
        var currentSubscription = await _dbContext.CompanySubscriptions.FirstAsync(subscription =>
            subscription.CompanyId == companySubscription.CompanyId);

        currentSubscription.SubscriptionPlan =
            await dbContext.SubscriptionsPlans.FirstAsync(plan => plan.Id == companySubscription.SubscriptionPlanId);
        _dbContext.CompanySubscriptions.Update(currentSubscription);
    }
    
    #region Write Only
    
    public async Task RegisterCompanyPlan(CompanySubscription companySubscription)
    {
        bool hasSubscription = await _dbContext
            .CompanySubscriptions
            .AsNoTracking()
            .AnyAsync(subscription => subscription.CompanyId == companySubscription.CompanyId);

        if (hasSubscription)
            await ChangePlanAsync(companySubscription);
        else
            await _dbContext.CompanySubscriptions.AddAsync(companySubscription);
    }

    #endregion
}
