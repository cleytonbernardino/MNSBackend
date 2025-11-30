using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Requests.ServicesDefinition;
using CommonTestUtilities.Tokens;
using MMS.Communication.Responses;
using MMS.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.ServiceDefinition.Update;

public class UpdateServiceDefinitionTest(CustomWebApplicationFactory factory): MmsClassFixture(factory)
{
    protected override string Method => "api/services";

    private readonly IdEncoderForTests _idEncoder = new();
    
    [Fact]
    public async Task Success()
    {
        var user = factory.EmployeeUser;
        
        var service = ServiceDefinitionBuilder.Build(companyId: user.CompanyId, registeredBy: user.UserIdentifier);
        factory.InjectInDatabase(service);
        
        string token = JwtTokenGeneratorBuilder.Build().Generate(user);
        
        var request = RequestUpdateServiceDefinitionBuilder.Build(id: _idEncoder.Encode(service.Id));

        var response = await DoPutAsync(request: request, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Service_Not_Found(string culture)
    {
        string token = JwtTokenGeneratorBuilder.Build().Generate(
            factory.EmployeeUser);

        var request = RequestUpdateServiceDefinitionBuilder.Build(id: _idEncoder.Encode(id: 10000));

        var response = await DoPutAsync(request: request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var errors = await response.Content.ReadFromJsonAsync<ResponseError>();
        errors.ShouldNotBeNull().Errors.ShouldHaveSingleItem().ShouldBe(
            ResourceMessagesException.ResourceManager.GetString("SERVICE_NOT_FOUND", new CultureInfo(culture)));
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Validator(string culture)
    {
        string token = JwtTokenGeneratorBuilder.Build().Generate(factory.EmployeeUser);
        
        var request = RequestUpdateServiceDefinitionBuilder.Build(id: string.Empty);

        var response = await DoPutAsync(request: request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await response.Content.ReadFromJsonAsync<ResponseError>();
        errors.ShouldNotBeNull().Errors.ShouldHaveSingleItem().ShouldBe(
            ResourceMessagesException.ResourceManager.GetString("EMPTY_SERVICES_ID", new CultureInfo(culture)));
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Unauthorized_Employee(string culture)
    {
        var service = ServiceDefinitionBuilder.Build(companyId: factory.ManagerUser.CompanyId,
            registeredBy: factory.ManagerUser.UserIdentifier);
        factory.InjectInDatabase(service);
        
        string token = JwtTokenGeneratorBuilder.Build().Generate(factory.EmployeeUser);
        
        var request = RequestUpdateServiceDefinitionBuilder.Build(id: _idEncoder.Encode(service.Id));

        var response = await DoPutAsync(request: request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        
        var errors = await response.Content.ReadFromJsonAsync<ResponseError>();
        errors.ShouldNotBeNull().Errors.ShouldHaveSingleItem().ShouldBe(
            ResourceMessagesException.ResourceManager.GetString("NO_PERMISSION", new CultureInfo(culture)));
    }
}
