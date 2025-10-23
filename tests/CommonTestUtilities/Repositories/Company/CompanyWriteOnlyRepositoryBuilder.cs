using MMS.Domain.Repositories.Company;
using Moq;

namespace CommonTestUtilities.Repositories.Company;

public static class CompanyWriteOnlyRepositoryBuilder
{
    public static ICompanyWriteOnlyRepository Build()
    {
        Mock<ICompanyWriteOnlyRepository> mock = new();
        return mock.Object;
    }
}
