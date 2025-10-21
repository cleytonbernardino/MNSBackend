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

namespace WebApi.Test.SubscriptionPlan.Register;

public class RegisterSubscriptionPlanTest(CustomWebApplicationFactory factory) : MmsClassFixture(factory)
{
    protected override string Method => "api/admin/plans";
    
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterSubscriptionPlanBuilder.Build();
        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);
        
        var response = await DoPostAsync(request: request, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }
    
    [Fact]
    public async Task Error_No_Permission()
    {
        var request = RequestRegisterSubscriptionPlanBuilder.Build();
        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);
        
        var response = await DoPostAsync(request: request, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Validator(string culture)
    {
        var request = RequestRegisterSubscriptionPlanBuilder.Build();
        request.Description = string.Empty;
        
        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);
        
        var response = await DoPostAsync(request: request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await response.Content.ReadFromJsonAsync<ResponseError>();
        errors!
            .Errors
            .ShouldHaveSingleItem()
            .ShouldBe(ResourceMessagesException.ResourceManager
                .GetString("DESCRIPTION_CANNOT_BE_EMPTY", new CultureInfo(culture)));
    }
}
