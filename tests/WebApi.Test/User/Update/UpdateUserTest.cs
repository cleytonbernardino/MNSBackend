using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MMS.Domain.Enums;
using MMS.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Update;

public class UpdateUserTest(CustomWebApplicationFactory factory) : MmsClassFixture(factory)
{
    private const string METHOD = "api/User";
    private readonly IdEncoderForTests _idEncoder = new();

    [Fact]
    public async Task Success()
    {
        string userToUpdate = InjectNewUser();

        var request = RequestUpdateUserBuilder.Build();
        request.UserIdToUpdate = userToUpdate;

        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);

        var response = await DoPutAsync(METHOD, request, token);
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Role_Invalid(string culture)
    {
        string userToUpdate = InjectNewUser();

        var request = RequestUpdateUserBuilder.Build();
        request.UserIdToUpdate = userToUpdate;
        request.Role = 1000;

        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);

        var response = await DoPutAsync(METHOD, request, token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await GetArrayFromResponse(response);
        errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("ROLE_INVALID", new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Without_Permission(string culture)
    {
        string userToUpdate = InjectNewUser();
        
        var request = RequestUpdateUserBuilder.Build();
        request.UserIdToUpdate = userToUpdate;

        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.EmployeeUser.UserIdentifier, UserRolesEnum.MANAGER);

        var response = await DoPutAsync(METHOD, request, token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        var errors = await GetArrayFromResponse(response);
        errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("NO_PERMISSION", new CultureInfo(culture)));
    }
    
    [Fact]
    public async Task Error_Authorize_Denies_Invalid_Roles()
    {
        var request = RequestUpdateUserBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.EmployeeUser.UserIdentifier, UserRolesEnum.EMPLOYEE);

        var response = await DoPutAsync(METHOD, request, token);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Email_Invalid(string culture)
    {
        string userToUpdate = InjectNewUser();

        var request = RequestUpdateUserBuilder.Build();
        request.UserIdToUpdate = userToUpdate;
        request.Email = "invalid";

        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);

        var response = await DoPutAsync(METHOD, request, token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await GetArrayFromResponse(response);
        errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("INVALID_EMAIL", new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Phone_Invalid(string culture)
    {
        string userToUpdate = InjectNewUser();

        var request = RequestUpdateUserBuilder.Build();
        request.UserIdToUpdate = userToUpdate;
        request.Phone = "ll 97578-27gg";

        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);

        var response = await DoPutAsync(METHOD, request, token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await GetArrayFromResponse(response);
        errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("PHONE_NOT_VALID", new CultureInfo(culture)));
    }

    private string InjectNewUser()
    {
        var user = UserBuilder.Build();
        factory.InjectUser(user);
        return _idEncoder.Encode(user.Id);
    }
}
