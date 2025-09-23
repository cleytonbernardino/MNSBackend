using CommonTestUtilities.Tokens;
using MMS.Communication.Responses.Company;
using MMS.Domain.Enums;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace WebApi.Test.Company.List;

public class ListCompaniesTest(CustomWebApplicationFactory factory) : MmsClassFixture(factory)
{
    protected override string Method => "api/admin/company";

    [Fact]
    public async Task Success()
    {
        var registeredCompanies = factory.RegisterCompanies();

        string token = JwtTokenGeneratorBuilder.Build().Generate(
            factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);

        var response = await DoGetAsync(token);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<ResponseShortCompanies>();
        result!.Companies.Count().ShouldBe(registeredCompanies.Count);
    }
}
