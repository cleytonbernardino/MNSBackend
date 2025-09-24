using CommonTestUtilities.Tokens;
using MMS.Domain.Enums;
using Shouldly;
using System.Net;
using WebApi.Test.InlineData;

namespace WebApi.Test.Company.Delete;

public class DeleteCompanyInvalidTokenTest(CustomWebApplicationFactory factory) : MmsClassFixture(factory)
{
    protected override string Method => "api/admin/company";

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Token(string culture)
    {
        string url = $"{Method}/yyy";
        
        var response = await DoDeleteAsync(token: "TokenInvalid", culture, customUrl:url);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Without_Token(string culture)
    {
        string url = $"{Method}/yyy";
        
        var response = await DoDeleteAsync(token: "", culture: culture, customUrl:url);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_Without_User(string culture)
    {
        string url = $"{Method}/yyy";
        string token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid(), UserRolesEnum.MANAGER);

        var response = await DoDeleteAsync(token, culture, customUrl:url);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}


