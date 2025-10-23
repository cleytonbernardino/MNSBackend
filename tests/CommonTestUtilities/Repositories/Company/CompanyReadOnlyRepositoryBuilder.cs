using CommonTestUtilities.Entities;
using MMS.Domain.Repositories.Company;
using Moq;
using Entity = MMS.Domain.Entities;

namespace CommonTestUtilities.Repositories.Company;

public class CompanyReadOnlyRepositoryBuilder
{
    private readonly Mock<ICompanyReadOnlyRepository> _mock = new();

    public ICompanyReadOnlyRepository Build() => _mock.Object;

    public CompanyReadOnlyRepositoryBuilder ListUsers(int amount = 5)
    {
        var users = ShortUserBuilder.BuildInBatch(amount);

        _mock.Setup(rep => rep.ListUsers(0)).Returns(users);
        return this;
    }
    
    public CompanyReadOnlyRepositoryBuilder ListShortCompanies(IList<Entity.ShortCompany> companiesToMock)
    {
        _mock.Setup(rep => rep.ListShortCompanies()).Returns(companiesToMock);
        return this;
    }

    public CompanyReadOnlyRepositoryBuilder Exists(bool exist)
    {
        _mock.Setup(mock => mock.Exists(It.IsAny<long>())).ReturnsAsync(exist);
        return this;
    }
}
