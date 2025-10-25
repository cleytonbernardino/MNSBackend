namespace MMS.Domain.Repositories.CompanySubscription;

public interface ICompanySubscriptionWriteOnlyRepository
{
    public Task RegisterCompanyPlan(Entities.CompanySubscription companySubscription);

}
