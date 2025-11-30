using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Tokens;
using MMS.Communication.Responses;
using MMS.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.ServiceDefinition.Delete;

public class DeleteServiceDefinitionTest(CustomWebApplicationFactory factory): MmsClassFixture(factory)
{
    protected override string Method => "api/services";

    private readonly IdEncoderForTests _idEncoder = new();

    [Fact]
    public async Task Success()
    {
        var user = factory.EmployeeUser;

        var service = ServiceDefinitionBuilder.Build();
        service.CompanyId = user.CompanyId;
        service.RegisteredBy = user.UserIdentifier;
        factory.InjectInDatabase(service);

        string id = _idEncoder.Encode(service.Id);
        string token = JwtTokenGeneratorBuilder.Build().Generate(user);

        var response = await DoDeleteAsync(token: token, parameter: id);
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task Success_Administrators_Can_ByPass()
    {
        var user = factory.ManagerUser;

        var service = ServiceDefinitionBuilder.Build();
        service.CompanyId = user.CompanyId;
        factory.InjectInDatabase(service);

        string id = _idEncoder.Encode(service.Id);
        string token = JwtTokenGeneratorBuilder.Build().Generate(user);

        var response = await DoDeleteAsync(token: token, parameter: id);
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Service_Definition_Not_Found(string culture)
    {
        var user = factory.ManagerUser;
        
        string id = _idEncoder.Encode(id: 10000);
        string token = JwtTokenGeneratorBuilder.Build().Generate(user);

        var response = await DoDeleteAsync(token: token, parameter: id, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var errors = await response.Content.ReadFromJsonAsync<ResponseError>();
        errors!.Errors.ShouldHaveSingleItem().ShouldBe(
            ResourceMessagesException.ResourceManager.GetString("SERVICE_NOT_FOUND", new CultureInfo(culture)));
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Unauthorized_Employee(string culture)
    {
        var user = factory.EmployeeUser;

        var service = ServiceDefinitionBuilder.Build();
        service.RegisteredBy = Guid.Empty;
        service.CompanyId = user.CompanyId;
        factory.InjectInDatabase(service);

        string id = _idEncoder.Encode(service.Id);
        string token = JwtTokenGeneratorBuilder.Build().Generate(user);

        var response = await DoDeleteAsync(token: token, parameter: id, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        var errors = await response.Content.ReadFromJsonAsync<ResponseError>();
        errors!.Errors.ShouldHaveSingleItem().ShouldBe(
            ResourceMessagesException.ResourceManager.GetString("NO_PERMISSION", new CultureInfo(culture)));
    }
}
