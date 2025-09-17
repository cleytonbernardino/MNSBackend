using CommonTestUtilities.Tokens;
using Shouldly;
using System.Globalization;
using System.Net;
using MMS.Communication;
using MMS.Domain.Enums;
using MMS.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Auth.RefreshToken;

public class RefreshTokenTest(CustomWebApplicationFactory factory) : MmsClassFixture(factory)
{
    private const string METHOD = "api/auth/refresh";
    
    [Fact]
    public async Task Success()
    {
        RequestRefreshAccessToken request = new()
        {
            AccessToken = JwtTokenGeneratorBuilder.Build().Generate(
                factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER
            )
        };

        var response = await DoPostWithRefreshTokenAsync(METHOD, request, factory.TokenRefresh.Token);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Access_Token_Empty(string culture)
    {
        RequestRefreshAccessToken request = new()
        {
            AccessToken = string.Empty
        };
        
        var response = await DoPostWithRefreshTokenAsync(METHOD, request, factory.TokenRefresh.Token, culture:culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await GetArrayFromResponse(response);
        errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("INVALID_ACCESS_TOKEN", new CultureInfo(culture)));
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Refresh_Token_Empty(string culture)
    {
        RequestRefreshAccessToken request = new()
        {
            AccessToken = JwtTokenGeneratorBuilder.Build().Generate(
                factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER
            ),
        };

        var response = await DoPostAsync(METHOD, request, culture:culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await GetArrayFromResponse(response);
        errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("NO_REFRESH_TOKEN", new CultureInfo(culture)));
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Refresh_Token_No_Registered(string culture)
    {
        RequestRefreshAccessToken request = new()
        {
            AccessToken = JwtTokenGeneratorBuilder.Build().Generate(
                factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER
            )
        };

        var response = await DoPostAsync(METHOD, request, culture:culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await GetArrayFromResponse(response);
        errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("NO_REFRESH_TOKEN", new CultureInfo(culture)));
    }
}
