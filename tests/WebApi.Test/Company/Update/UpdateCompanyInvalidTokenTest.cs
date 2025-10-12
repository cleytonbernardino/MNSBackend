using CommonTestUtilities.Tokens;
using MMS.Communication.Requests.Company;
using MMS.Domain.Enums;
using Shouldly;
using System.Net;
using WebApi.Test.InlineData;

namespace WebApi.Test.Company.Update;

public class UpdateCompanyInvalidTokenTest(CustomWebApplicationFactory factory) : MmsClassFixture(factory)
{
    protected override string Method => "api/admin/company/yyy";

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Token(string culture)
    {
        RequestUpdateCompany request = new();
        
        var response = await DoPutAsync(request, token: "TokenInvalid", culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Without_Token(string culture)
    {
        RequestUpdateCompany request = new();
        
        var response = await DoPutAsync(request,token: "", culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Token_Without_User(string culture)
    {
        string token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid(), UserRolesEnum.MANAGER);
        RequestUpdateCompany request = new();
        
        var response = await DoPutAsync(request,token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}


