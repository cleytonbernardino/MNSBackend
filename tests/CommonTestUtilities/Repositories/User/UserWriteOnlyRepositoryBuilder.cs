using MMS.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories.User;

public static class UserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Build()
    {
        Mock<IUserWriteOnlyRepository> mock = new();
        return mock.Object;
    }
}
