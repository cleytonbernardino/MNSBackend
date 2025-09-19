using CommonTestUtilities.Requests;
using MMS.Communication.Requests.Auth;
using MMS.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using WebApi.Test.InlineData;

namespace WebApi.Test.Auth.DoLogin;

public class DoLoginTest(
    CustomWebApplicationFactory factory
) : MmsClassFixture(factory)
{
    private const string METHOD = "api/auth/login";

    [Fact]
    public async Task Success()
    {
        RequestDoLogin request = new() { Email = factory.ManagerUser.Email, Password = factory.UserPassword };

        var response = await DoPostAsync(METHOD, request);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Email_Credentials(string culture)
    {
        RequestDoLogin request = new() { Email = "tes@email.com", Password = factory.UserPassword };

        var response = await DoPostAsync(METHOD, request, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        var errorList = await GetArrayFromResponse(response);

        errorList
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID",
                new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Password_Credentials(string culture)
    {
        RequestDoLogin request = new() { Email = factory.ManagerUser.Email, Password = factory.UserPassword + "abc" };

        var response = await DoPostAsync(METHOD, request, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        var errorList = await GetArrayFromResponse(response);

        errorList
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID",
                new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Email(string culture)
    {
        RequestDoLogin request = RequestLoginBuilder.Build();
        request.Email = "tes";

        var response = await DoPostAsync(METHOD, request, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errorList = await GetArrayFromResponse(response);

        errorList
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("INVALID_EMAIL", new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Password(string culture)
    {
        RequestDoLogin request = RequestLoginBuilder.Build();
        request.Password = string.Empty;

        var response = await DoPostAsync(METHOD, request, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errorList = await GetArrayFromResponse(response);

        errorList
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("PASSWORD_EMPTY", new CultureInfo(culture)));
    }
}
