using Bogus;
using MMS.Communication;

namespace CommonTestUtilities.Requests;

public static class RequestRefreshTokenBuilder
{
    public static RequestRefreshToken Build()
    {
        return new Faker<RequestRefreshToken>()
            .RuleFor(req => req.AccessToken, f => f.Lorem.Sentence(3))
            .RuleFor(req => req.RefreshToken, f => f.Lorem.Sentence(3));
    }
}
