using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MMS.Communication.Responses;
using MMS.Domain.Enums;
using MMS.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register;

public class RegisterUserTest(
    CustomWebApplicationFactory factory
    ) : MmsClassFixture(factory)
{
    protected override string Method => "api/user";

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);

        var request = RequestRegisterUseBuilder.Build();

        var response = await DoPostAsync(request, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    public async Task Success_Without_Last_Name(string lastName)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);

        var request = RequestRegisterUseBuilder.Build();
        request.LastName = lastName;

        var response = await DoPostAsync(request, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Email_In_Use(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);

        var request = RequestRegisterUseBuilder.Build();
        request.Email = factory.ManagerUser.Email;

        var response = await DoPostAsync(request: request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errorList = await response.Content.ReadFromJsonAsync<ResponseError>();
        errorList!.Errors
            .ShouldHaveSingleItem().ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("EMAIL_IN_USE", new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Validator(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);

        var request = RequestRegisterUseBuilder.Build();
        request.Phone = string.Empty;

        var response = await DoPostAsync(request: request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errorList = await response.Content.ReadFromJsonAsync<ResponseError>();
        errorList!.Errors
            .ShouldHaveSingleItem().ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("PHONE_EMPTY", new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_User_Does_Not_Have_Permission(string culture)
    {
        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.EmployeeUser.UserIdentifier, UserRolesEnum.EMPLOYEE);

        var request = RequestRegisterUseBuilder.Build();

        var response = await DoPostAsync(request: request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
