using MMS.Domain.Repositories.Company;
using Moq;
using Entity = MMS.Domain.Entities;

namespace CommonTestUtilities.Repositories.Company;

public class CompanyUpdateOnlyRepositoryBuilder
{
    private readonly Mock<ICompanyUpdateOnlyRepository> _mock = new();

    public ICompanyUpdateOnlyRepository Build()
    {
        MockFunctionsWithoutReturns();
        return _mock.Object;
    }

    public CompanyUpdateOnlyRepositoryBuilder GetById(Entity.Company company)
    {
        _mock.Setup(mock => mock.GetById(company.Id)).ReturnsAsync(company);
        return this;
    }

    private void MockFunctionsWithoutReturns()
    {
        _mock.Setup(mock => mock.Update(It.IsAny<Entity.Company>()));
    }
}
