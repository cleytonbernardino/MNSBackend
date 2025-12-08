using CommonTestUtilities.Entities;
using CommonTestUtilities.Tokens;
using MMS.Communication.Responses;
using MMS.Communication.Responses.ServiceDefinition;
using MMS.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.ServiceDefinition.List;

public class ListServiceDefinitionTest(CustomWebApplicationFactory factory): MmsClassFixture(factory)
{
    protected override string Method => "api/services";

    [Fact]
    public async Task Success()
    {
        var user = factory.ManagerUser;
        
        var services = ServiceDefinitionBuilder.BuildInBatch(companyId: user.CompanyId);
        factory.InjectInDatabase(services);
        
        string token = JwtTokenGeneratorBuilder.Build().Generate(user);
        
        var response = await DoGetAsync(token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<ResponseShortServicesDefinition>().ShouldNotBeNull();
        result!.ServiceDefinitions.Length.ShouldBe(services.Length);
        result!.ServiceDefinitions.First().Title.ShouldBe(services[0].Title);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_No_Permission(string culture)
    {
        string token = JwtTokenGeneratorBuilder.Build().Generate(factory.EmployeeUser);
        
        var response = await DoGetAsync(token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        var result = await response.Content.ReadFromJsonAsync<ResponseError>().ShouldNotBeNull();
        result!.Errors.ShouldHaveSingleItem().ShouldBe(
            ResourceMessagesException.ResourceManager.GetString("NO_PERMISSION", new CultureInfo(culture)));
    }
}
