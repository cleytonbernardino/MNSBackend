using Bogus;
using MMS.Communication;

namespace CommonTestUtilities.Requests;

public static class RequestLoginBuilder
{
    public static RequestLogin Build()
    {
        return new Faker<RequestLogin>()
            .RuleFor(user => user.Email, f => f.Internet.Email())
            .RuleFor(user => user.Password, f => f.Internet.Password());
    }
}
