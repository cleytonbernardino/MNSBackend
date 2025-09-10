using MMS.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;

public static class UserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Build()
    {
        Mock<IUserWriteOnlyRepository> mock = new();
        return mock.Object;
    }
}
