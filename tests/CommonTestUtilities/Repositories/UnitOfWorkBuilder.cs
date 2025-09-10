using MMS.Domain.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories;

public static class UnitOfWorkBuilder
{
    public static IUnitOfWork Build()
    {
        Mock<IUnitOfWork> mock = new();
        return mock.Object;
    }
}
