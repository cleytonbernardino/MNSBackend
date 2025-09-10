using MMS.Domain.Entities;
using MMS.Domain.Services.LoggedUser;
using Moq;

namespace CommonTestUtilities.Services.LoggedUser;

public static class LoggedUserBuilder
{
    public static ILoggedUser Build(User user)
    {
        var mock = new Mock<ILoggedUser>();
        mock.Setup(mock => mock.User()).ReturnsAsync(user);
        return mock.Object;
    }
}
