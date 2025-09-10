using Bogus;
using MMS.Domain.Entities;

namespace CommonTestUtilities.Entities;

public static class RefreshTokenBuilder
{
    public static RefreshToken Build()
    {
        return new Faker<RefreshToken>()
            .RuleFor(token => token.Id, () => 1)
            .RuleFor(token => token.UserIdentifier, Guid.NewGuid())
            .RuleFor(token => token.Token, f => f.Random.AlphaNumeric(10))
            .RuleFor(token => token.ExpiryDate, () => DateTime.UtcNow.AddHours(1));
    }
}
