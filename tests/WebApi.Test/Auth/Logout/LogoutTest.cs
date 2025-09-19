using CommonTestUtilities.Tokens;
using MMS.Communication.Requests.Auth;
using MMS.Domain.Enums;
using Shouldly;
using System.Net;

namespace WebApi.Test.Auth.Logout;

public class LogoutTest(CustomWebApplicationFactory factory) : MmsClassFixture(factory)
{
    private const string METHOD = "api/auth/logout";

    [Fact]
    public async Task Success()
    {
        RequestRefreshToken request = new()
        {
            AccessToken = JwtTokenGeneratorBuilder.Build().Generate(
                factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER
            )
        };

        HttpResponseMessage response = await DoPostWithRefreshTokenAsync(
            METHOD, request, factory.TokenRefresh.Token!
        );
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Error_No_Refresh_Token()
    {
        RequestRefreshToken request = new()
        {
            AccessToken = JwtTokenGeneratorBuilder.Build().Generate(
                factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER
            )
        };

        HttpResponseMessage response = await DoPostAsync(METHOD, request);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
