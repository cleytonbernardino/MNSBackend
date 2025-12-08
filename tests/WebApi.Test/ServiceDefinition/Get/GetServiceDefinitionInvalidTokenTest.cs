using CommonTestUtilities.Tokens;
using MMS.Domain.Enums;
using Shouldly;
using System.Net;
using WebApi.Test.InlineData;

namespace WebApi.Test.ServiceDefinition.Get;

public class GetServiceDefinitionInvalidTokenTest(CustomWebApplicationFactory factory) : MmsClassFixture(factory)
{
    protected override string Method => "api/services";

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Token(string culture)
    {
        var response = await DoGetAsync(parameter: "yyy", token: "TokenInvalid", culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Without_Token(string culture)
    {
        var response = await DoGetAsync(parameter: "yyy", token: "", culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_Without_User(string culture)
    {
        string token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid(), UserRolesEnum.MANAGER);

        var response = await DoGetAsync(parameter: "yyy", token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
