using Microsoft.EntityFrameworkCore;
using MMS.Domain.Entities;
using MMS.Domain.Repositories.SubscriptionPlan;

namespace MMS.Infrastructure.DataAccess.Repositories;

public class SubscriptionPlanRepository(
    MmsDbContext dbContext
    ) : ISubscriptionPlanWriteOnlyRepository
{
    private readonly MmsDbContext _dbContext = dbContext;
    
    public async Task Register(SubscriptionsPlan subscriptionPlan)
    {
        await _dbContext.SubscriptionsPlans.AddAsync(subscriptionPlan);
    }
}
