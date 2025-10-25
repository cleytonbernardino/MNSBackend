using MMS.Domain.Repositories.CompanySubscription;
using Moq;

namespace CommonTestUtilities.Repositories.CompanySubscription;

public static class CompanySubscriptionWriteOnlyBuilder
{
    public static ICompanySubscriptionWriteOnlyRepository Build()
    {
        Mock<ICompanySubscriptionWriteOnlyRepository> mock = new();
        return mock.Object;
    }
}
