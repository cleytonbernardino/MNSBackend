using Bogus;
using MMS.Communication.Requests.Auth;

namespace CommonTestUtilities.Requests;

public static class RequestLoginBuilder
{
    public static RequestDoLogin Build()
    {
        return new Faker<RequestDoLogin>()
            .RuleFor(user => user.Email, f => f.Internet.Email())
            .RuleFor(user => user.Password, f => f.Internet.Password());
    }
}
