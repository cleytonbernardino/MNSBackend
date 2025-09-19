using Bogus;
using MMS.Communication.Requests.Auth;

namespace CommonTestUtilities.Requests;

public static class RequestRefreshTokenBuilder
{
    public static RequestRefreshToken Build()
    {
        return new Faker<RequestRefreshToken>()
            .RuleFor(req => req.AccessToken, f => f.Lorem.Sentence(3));
    }
}
