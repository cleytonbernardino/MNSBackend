using Bogus;
using CommonTestUtilities.Cryptography;
using MMS.Domain.Entities;
using MMS.Domain.Enums;

namespace CommonTestUtilities.Entities;

public static class UserBuilder
{
    private static Faker<User> GenerateUser(bool isAdmin)
    {
        return new Faker<User>()
            .RuleFor(user => user.Id, 0)
            .RuleFor(user => user.UpdatedOn, () => DateTime.UtcNow)
            .RuleFor(user => user.LastLogin, () => DateTime.UtcNow)
            .RuleFor(user => user.IsAdmin, isAdmin)
            .RuleFor(user => user.UserIdentifier, Guid.NewGuid())
            .RuleFor(user => user.Email, f => f.Internet.Email())
            .RuleFor(user => user.Phone, () => "(11) 981628391")
            .RuleFor(user => user.FirstName, f => f.Name.FirstName())
            .RuleFor(user => user.LastName, f => f.Name.LastName())
            .RuleFor(user => user.Role, () => UserRolesEnum.MANAGER);
    }

    public static User Build(bool isAdmin = false)
    {
        return GenerateUser(isAdmin)
            .RuleFor(user => user.Password, f => f.Internet.Password());
    }

    public static User[] BuildInBatch(uint count = 5)
    {
        List<User> users = [];
        for (uint i = 0; i < count; i++)
        {
            var user = Build();
            users.Add(user);
        }
        return users.ToArray();
    }
    
    public static (User user, string password) BuildWithPassword()
    {
        var password = new Faker().Internet.Password();

        return (
            user: GenerateUser(isAdmin: false).RuleFor(user => user.Password, () =>
            {
                var passwordEncrypter = PasswordEncrypterBuilder.Build();
                return passwordEncrypter.Encrypt(password);
            }),
            password: password
        );
    }
}
