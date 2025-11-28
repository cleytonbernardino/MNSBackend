using CommonTestUtilities.Requests;
using CommonTestUtilities.Requests.ServicesDefinition;
using CommonTestUtilities.Tokens;
using MMS.Communication.Responses;
using MMS.Domain.Enums;
using MMS.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.ServiceDefinition.Register;

public class RegisterCompanyServicesTest(CustomWebApplicationFactory factory) : MmsClassFixture(factory)
{
    protected override string Method => "api/services";
 
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterServiceDefinitionBuilder.Build();

        string token = JwtTokenGeneratorBuilder.Build().Generate(
            factory.EmployeeUser.UserIdentifier, UserRolesEnum.EMPLOYEE);

        var response = await DoPostAsync(request: request, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }
    
    [Fact]
    public async Task Success_No_Status()
    {
        var request = RequestRegisterServiceDefinitionBuilder.Build();
        request.Status = null;

        string token = JwtTokenGeneratorBuilder.Build().Generate(
            factory.EmployeeUser.UserIdentifier, UserRolesEnum.EMPLOYEE);

        var response = await DoPostAsync(request: request, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }
    
    [Fact]
    public async Task Success_No_Description()
    {
        var request = RequestRegisterServiceDefinitionBuilder.Build();
        request.Description = string.Empty;

        string token = JwtTokenGeneratorBuilder.Build().Generate(
            factory.EmployeeUser.UserIdentifier, UserRolesEnum.EMPLOYEE);

        var response = await DoPostAsync(request: request, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Validator(string culture)
    {
        var request = RequestRegisterServiceDefinitionBuilder.Build();
        request.Title = String.Empty;

        string token = JwtTokenGeneratorBuilder.Build().Generate(
            factory.EmployeeUser.UserIdentifier, UserRolesEnum.EMPLOYEE);

        var response = await DoPostAsync(request: request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await response.Content.ReadFromJsonAsync<ResponseError>();
        errors.ShouldNotBeNull()
            .Errors.ShouldHaveSingleItem().ShouldBe(
            ResourceMessagesException.ResourceManager.GetString("EMPTY_TITLE", new CultureInfo(culture)));
    }
    

}
