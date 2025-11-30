using CommonTestUtilities.Cryptography;
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

namespace WebApi.Test.ServiceDefinition.Get;

public class GetServiceDefinitionTest(CustomWebApplicationFactory factory): MmsClassFixture(factory)
{
    protected override string Method => "api/services";

    private readonly IdEncoderForTests _idEncoder = new();
    
    [Fact]
    public async Task Success()
    {
        var user = factory.EmployeeUser;

        var service = ServiceDefinitionBuilder.Build(
            companyId: user.CompanyId, registeredBy: user.UserIdentifier);
        factory.InjectInDatabase(service);
        
        string token = JwtTokenGeneratorBuilder.Build().Generate(user);
        string id = _idEncoder.Encode(service.Id);
        
        var response = await DoGetAsync(token: token, parameter: id);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<ResponseGetServiceDefinition>();
        result?.Title.ShouldBe(service.Title);
    }
    
    [Fact]
    public async Task Success_Administrators_Can_ByPass()
    {
        var user = factory.ManagerUser;

        var service = ServiceDefinitionBuilder.Build(companyId: user.CompanyId);
        factory.InjectInDatabase(service);
        
        string token = JwtTokenGeneratorBuilder.Build().Generate(user);
        string id = _idEncoder.Encode(service.Id);
        
        var response = await DoGetAsync(token: token, parameter: id);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<ResponseGetServiceDefinition>();
        result?.Title.ShouldBe(service.Title);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Service_Not_Found(string culture)
    {
        var user = factory.ManagerUser;
        var service = ServiceDefinitionBuilder.Build(companyId: user.CompanyId);

        string token = JwtTokenGeneratorBuilder.Build().Generate(user);
        string id = _idEncoder.Encode(id: 10000);

        var response = await DoGetAsync(token: token, parameter: id, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var result = await response.Content.ReadFromJsonAsync<ResponseError>();
        result.ShouldNotBeNull()
            .Errors.ShouldHaveSingleItem()
            .ShouldBe(
                ResourceMessagesException.ResourceManager.GetString("SERVICE_NOT_FOUND", new CultureInfo(culture)));
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Unauthorized_Employee(string culture)
    {
        var user = factory.EmployeeUser;
        
        var service = ServiceDefinitionBuilder.Build(
            companyId: user.CompanyId, registeredBy:factory.AdminUser.UserIdentifier);
        factory.InjectInDatabase(service);

        string token = JwtTokenGeneratorBuilder.Build().Generate(user);
        string id = _idEncoder.Encode(id: service.Id);

        var response = await DoGetAsync(token: token, parameter: id, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        var result = await response.Content.ReadFromJsonAsync<ResponseError>();
        result.ShouldNotBeNull()
            .Errors.ShouldHaveSingleItem()
            .ShouldBe(
                ResourceMessagesException.ResourceManager.GetString("NO_PERMISSION", new CultureInfo(culture)));
    }
}
