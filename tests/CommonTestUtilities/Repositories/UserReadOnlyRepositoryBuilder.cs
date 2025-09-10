using Moq;
using MMS.Domain.Repositories.User;
using Entity = MMS.Domain.Entities;

namespace CommonTestUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _mock = new();

    public IUserReadOnlyRepository Build() => _mock.Object;

    public void ExistActiveUserWithEmail(string email)
    {
        _mock.Setup(repository => repository.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
    }

    public void GetUserByEmailAndPassword(Entity.User user)
    {
        _mock.Setup(repository => repository.GetUserByEmailAndPassword(user.Email, user.Password)).ReturnsAsync(user);
    }
}
