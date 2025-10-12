using Bogus;
using MMS.Domain.Entities;
using MMS.Domain.Enums;

namespace CommonTestUtilities.Entities;

public static class ShortUserBuilder
{
    private static ShortUser Build()
    {
        return new Faker<ShortUser>()
            .RuleFor(user => user.Id, f => f.Random.Int(1, 1000))
            .RuleFor(user => user.Active, true)
            .RuleFor(user => user.FirstName, f => f.Name.FirstName())
            .RuleFor(user => user.Email, f => f.Internet.Email())
            .RuleFor(user => user.Role, () => UserRolesEnum.MANAGER);
    }

    public static ShortUser[] BuildInBatch(int amount = 5)
    {
        List<ShortUser> users = new();
        for (int i = 0; i < amount; i++)
        {
            users.Add(Build());
        }
        return users.ToArray();
    }
}
