using Bogus;
using MMS.Communication;

namespace CommonTestUtilities.Requests;

public static class RequestRefreshTokenBuilder
{
    public static RequestRefreshAccessToken Build()
    {
        return new Faker<RequestRefreshAccessToken>()
            .RuleFor(req => req.AccessToken, f => f.Lorem.Sentence(3));
    }
}
