using MMS.Domain.Repositories.User;
using Moq;
using Entity = MMS.Domain.Entities;

namespace CommonTestUtilities.Repositories.User;

public class UserUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IUserUpdateOnlyRepository> _mock = new();

    public IUserUpdateOnlyRepository Build() => _mock.Object;

    public UserUpdateOnlyRepositoryBuilder GetById(long userIdToMock, Entity.User mockUserReturn)
    {
        _mock.Setup(rep => rep.GetById(userIdToMock, mockUserReturn.CompanyId)).ReturnsAsync(mockUserReturn);
        return this;
    }

    public UserUpdateOnlyRepositoryBuilder GetUserByEmailAndPassword(Entity.User user, string password)
    {
        _mock.Setup(rep => rep.GetUserByEmailAndPassword(user.Email, password)).ReturnsAsync(user);
        return this;
    }
}
