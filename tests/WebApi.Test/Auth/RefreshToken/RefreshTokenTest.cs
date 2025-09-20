using CommonTestUtilities.Tokens;
using MMS.Communication.Responses;
using MMS.Domain.Enums;
using MMS.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Auth.RefreshToken;

public class RefreshTokenTest(CustomWebApplicationFactory factory) : MmsClassFixture(factory)
{
    private const string METHOD = "api/auth/refresh";
    
    [Fact]
    public async Task Success()
    {
        string accessToken = JwtTokenGeneratorBuilder.Build().Generate(
            factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);
    
        var response = await DoGetWithRefreshTokenAsync(
            METHOD, accessToken: accessToken, refreshToken: factory.TokenRefresh.Token
            );
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Refresh_Token_Empty(string culture)
    {
        string accessToken = JwtTokenGeneratorBuilder.Build().Generate(
            factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);
        string refreshToken = string.Empty;
        
        var response = await DoGetWithRefreshTokenAsync(METHOD, refreshToken, accessToken, culture:culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        
        var errors = await response.Content.ReadFromJsonAsync<ResponseError>();;
        errors!.Errors
            .ShouldHaveSingleItem()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("NO_REFRESH_TOKEN", new CultureInfo(culture)));
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Refresh_Token_No_Registered(string culture)
    {
        string accessToken = JwtTokenGeneratorBuilder.Build().Generate(
            factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);
        string refreshToken = "Invalid refresh Token";

        var response = await DoGetWithRefreshTokenAsync(METHOD, refreshToken, accessToken, culture:culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await response.Content.ReadFromJsonAsync<ResponseError>();;
        errors!.Errors
            .ShouldHaveSingleItem()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("NO_REFRESH_TOKEN", new CultureInfo(culture)));
    }
}
