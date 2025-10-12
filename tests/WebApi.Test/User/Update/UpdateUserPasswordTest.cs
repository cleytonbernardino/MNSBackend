using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MMS.Communication.Requests.User;
using MMS.Communication.Responses;
using MMS.Domain.Enums;
using MMS.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Update;

public class UpdateUserPasswordTest(CustomWebApplicationFactory factory) : MmsClassFixture(factory)
{
    protected override string Method => "api/user/change-password";

    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserPasswordBuilder.Build();
        request.OldPassword = factory.UserPassword;
        
        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);

        var response = await DoPutAsync(request, token);
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

        var response = await DoPutAsync(request, token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        var errors = await response.Content.ReadFromJsonAsync<ResponseError>();
        errors!.Errors
            .ShouldHaveSingleItem()
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

        var response = await DoPutAsync(request, token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await response.Content.ReadFromJsonAsync<ResponseError>();
        errors!.Errors
            .ShouldHaveSingleItem()
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

        var response = await DoPutAsync(request, token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await response.Content.ReadFromJsonAsync<ResponseError>();
        errors!.Errors
            .ShouldHaveSingleItem()
            .ShouldBe(
                ResourceMessagesException.ResourceManager.GetString("PASSWORD_LENGTH_IS_INVALID", new CultureInfo(culture))
                );
    }
}
