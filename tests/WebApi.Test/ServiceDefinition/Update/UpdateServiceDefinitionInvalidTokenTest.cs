using CommonTestUtilities.Requests.ServicesDefinition;
using CommonTestUtilities.Tokens;
using MMS.Communication.Requests.ServiceDefinition;
using MMS.Domain.Enums;
using Shouldly;
using System.Net;
using WebApi.Test.InlineData;

namespace WebApi.Test.ServiceDefinition.Update;

public class UpdateServiceDefinitionInvalidTokenTest(CustomWebApplicationFactory factory) : MmsClassFixture(factory)
{
    protected override string Method => "api/services";

    private readonly RequestUpdateServiceDefinition _request = RequestUpdateServiceDefinitionBuilder.Build();

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Token(string culture)
    {
        var response = await DoPutAsync(request: _request, token: "TokenInvalid", culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Without_Token(string culture)
    {
        var response = await DoPutAsync(request: _request, token: "", culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_Without_User(string culture)
    {
        string token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid(), UserRolesEnum.MANAGER);

        var response = await DoPutAsync(request: _request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
