using Microsoft.EntityFrameworkCore;
using MMS.Domain.Entities;
using MMS.Domain.Repositories.SubscriptionPlan;

namespace MMS.Infrastructure.DataAccess.Repositories;

public class SubscriptionPlanRepository(
    MmsDbContext dbContext
    ) : ISubscriptionPlanWriteOnlyRepository, ISubscriptionPlanReadOnlyRepository, ISubscriptionPlanUpdateOnlyRepository
{
    #region Dependency Injection

    private readonly MmsDbContext _dbContext = dbContext;

    #endregion
    
    #region Write Only

    public async Task Register(SubscriptionsPlan subscriptionPlan)
    {
        await _dbContext.SubscriptionsPlans.AddAsync(subscriptionPlan);
    }

    #endregion
    
    #region Read Only

    public async Task<SubscriptionsPlan[]> List()
    {
        return await _dbContext.SubscriptionsPlans.AsNoTracking().ToArrayAsync();
    }

    public async Task<bool> Exists(short id)
    {
        return await _dbContext.SubscriptionsPlans.AsNoTracking().AnyAsync(plan => plan.Id == id);
    }

    #endregion

    #region Update Only

    public async Task<SubscriptionsPlan?> GetById(short id)
    {
        return await _dbContext.SubscriptionsPlans
            .FirstOrDefaultAsync(subscriptionPlans =>
                subscriptionPlans.Id == id);
    }

    public void Update(SubscriptionsPlan subscriptionPlan)
    {
        _dbContext.SubscriptionsPlans.Update(subscriptionPlan);
    }

    public async Task Delete(short id)
    {
        var plan = await GetById(id);
        if (plan is null)
            return;
        _dbContext.SubscriptionsPlans.Remove(plan);
    }

    #endregion
}
