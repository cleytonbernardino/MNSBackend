using CommonTestUtilities.Tokens;
using MMS.Communication.Responses.Company;
using MMS.Domain.Enums;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Company.List;

public class ListCompaniesTest(CustomWebApplicationFactory factory) : MmsClassFixture(factory)
{
    private const string METHOD = "api/admin/company";

    [Fact]
    public async Task Success()
    {
        IList<MMS.Domain.Entities.Company> registeredCompanies = factory.RegisterCompanies();

        string token = JwtTokenGeneratorBuilder.Build().Generate(
            factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);

        HttpResponseMessage response = await DoGetAsync(METHOD, token);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        string jsonString = await response.Content.ReadAsStringAsync();

        JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        ResponseShortCompanies? s = JsonSerializer.Deserialize<ResponseShortCompanies>(jsonString, options);

        JsonElement.ArrayEnumerator result = await GetArrayFromResponse(response, "companies");
        result.Count().ShouldBe(registeredCompanies.Count);
    }
}
