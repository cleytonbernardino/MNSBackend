using MMS.Domain.Entities;
using MMS.Domain.Repositories.Company;
using Moq;

namespace CommonTestUtilities.Repositories;

public class CompanyUpdateOnlyRepositoryBuilder
{
    private readonly Mock<ICompanyUpdateOnlyRepository> _mock = new();

    public ICompanyUpdateOnlyRepository Build()
    {
        MockFunctionsWithoutReturns();
        return _mock.Object;
    }

    public CompanyUpdateOnlyRepositoryBuilder GetById(Company company)
    {
        _mock.Setup(mock => mock.GetById(company.Id)).ReturnsAsync(company);
        return this;
    }

    private void MockFunctionsWithoutReturns()
    {
        _mock.Setup(mock => mock.Update(It.IsAny<Company>()));
    }
}
