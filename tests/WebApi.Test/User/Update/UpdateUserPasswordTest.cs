using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;
using System.Globalization;
using System.Net;
using MMS.Communication;
using MMS.Domain.Enums;
using MMS.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Update;

public class UpdateUserPasswordTest(CustomWebApplicationFactory factory) : MmsClassFixture(factory)
{
    private const string METHOD = "user/change-password";

    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserPasswordBuilder.Build();
        request.OldPassword = factory.UserPassword;
        
        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);

        var response = await DoPutAsync(METHOD, request, token);
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Current_Password_Incorrect(string culture)
    {
        var request = RequestUpdateUserPasswordBuilder.Build();
        request.OldPassword = "WrongPassword123!";
        
        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);

        var response = await DoPutAsync(METHOD, request, token, culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        var errors = await GetArrayFromResponse(response);
        errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("CURRENT_PASSWORD_INCORRECT", new CultureInfo(culture)));
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Password_Empty(string culture)
    {
        RequestUpdateUserPassword request = new()
        {
            OldPassword = factory.UserPassword, 
            NewPassword = string.Empty
        };
        
        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);

        var response = await DoPutAsync(METHOD, request, token, culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await GetArrayFromResponse(response);
        errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("PASSWORD_EMPTY", new CultureInfo(culture)));
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Password_Too_Short(string culture)
    {
        RequestUpdateUserPassword request = new()
        {
            OldPassword = factory.UserPassword, 
            NewPassword = "ab!"
        };
        
        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);

        var response = await DoPutAsync(METHOD, request, token, culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await GetArrayFromResponse(response);
        errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(
                ResourceMessagesException.ResourceManager.GetString("PASSWORD_LENGTH_IS_INVALID", new CultureInfo(culture))
                );
    }
}
