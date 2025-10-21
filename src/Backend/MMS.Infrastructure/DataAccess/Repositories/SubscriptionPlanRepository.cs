using Microsoft.EntityFrameworkCore;
using MMS.Domain.Entities;
using MMS.Domain.Repositories.SubscriptionPlan;

namespace MMS.Infrastructure.DataAccess.Repositories;

public class SubscriptionPlanRepository(
    MmsDbContext dbContext
    ) : ISubscriptionPlanWriteOnlyRepository, ISubscriptionPlanReadOnlyRepository, ISubscriptionPlanUpdateOnlyRepository
{
    private readonly MmsDbContext _dbContext = dbContext;
    
    public async Task Register(SubscriptionsPlan subscriptionPlan)
    {
        await _dbContext.SubscriptionsPlans.AddAsync(subscriptionPlan);
    }

    public async Task<SubscriptionsPlan[]> List()
    {
        return await _dbContext.SubscriptionsPlans.ToArrayAsync();
    }

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
}
