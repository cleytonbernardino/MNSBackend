using CommonTestUtilities.Tokens;
using MMS.Communication.Requests.Auth;
using MMS.Communication.Responses;
using MMS.Domain.Enums;
using MMS.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Auth.Logout;

public class LogoutTest(CustomWebApplicationFactory factory) : MmsClassFixture(factory)
{
    protected override string Method => "api/auth/logout";

    [Fact]
    public async Task Success()
    {
        string accessToken = JwtTokenGeneratorBuilder.Build().Generate(
            factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);

        string refreshToken = factory.TokenRefresh.Token!;

        var response = await DoGetWithRefreshTokenAsync(refreshToken, accessToken);
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_No_Refresh_Token(string culture)
    {
        string accessToken = JwtTokenGeneratorBuilder.Build().Generate(
            factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);

        var response = await DoGetAsync(accessToken, culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        
        var errors = await response.Content.ReadFromJsonAsync<ResponseError>();;
        errors!.Errors
            .ShouldHaveSingleItem()
            .ShouldBe(
                ResourceMessagesException.ResourceManager
                    .GetString("NO_REFRESH_TOKEN", new CultureInfo(culture))
                );
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_No_Access_Token(string culture)
    {
        string accessToken = string.Empty;
        string refreshToken = factory.TokenRefresh.Token!;

        var response = await DoGetWithRefreshTokenAsync(refreshToken, accessToken, culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        
        var errors = await response.Content.ReadFromJsonAsync<ResponseError>();;
        errors!.Errors
            .ShouldHaveSingleItem()
            .ShouldBe(
                ResourceMessagesException.ResourceManager
                    .GetString("INVALID_ACCESS_TOKEN", new CultureInfo(culture))
            );
    }
}
