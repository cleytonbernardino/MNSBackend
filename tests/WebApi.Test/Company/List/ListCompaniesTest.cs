using CommonTestUtilities.Entities;
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
        var companies = CompanyBuilder.BuildInBatch(count: 10);
        foreach (var company in companies)
        {
            company.Id = 0;
        }
        
        var registeredCompanies = factory.InjectInDatabase(companies);

        string token = JwtTokenGeneratorBuilder.Build().Generate(
            factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);

        var response = await DoGetAsync(token);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<ResponseShortCompanies>();
        result!.Companies.Count().ShouldBe(registeredCompanies.Length);
    }
}
