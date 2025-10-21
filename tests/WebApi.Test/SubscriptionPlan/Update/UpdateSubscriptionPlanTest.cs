using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
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
using Entity = MMS.Domain.Entities;

namespace WebApi.Test.SubscriptionPlan.Update;

public class UpdateSubscriptionPlanTest(CustomWebApplicationFactory factory) : MmsClassFixture(factory)
{
    protected override string Method => "api/admin/plans";
    
    private readonly IdEncoderForTests _idEncoder = new();
    
    [Fact]
    public async Task Success()
    {
        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);
    
        var subscriptionPlan = SubscriptionPlanBuilder.Build();
        factory.InjectInDatabase<short, Entity.SubscriptionsPlan>([subscriptionPlan]);
    
        var request = RequestUpdateSubscriptionPlanBuilder.Build();
        request.Id = _idEncoder.Encode(subscriptionPlan.Id);
        
        var response = await DoPutAsync(request: request, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Error_No_Permission()
    {
        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);
        
        var request = RequestUpdateSubscriptionPlanBuilder.Build();
        
        var response = await DoPutAsync(request: request, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Subscription_Plan_Not_Found(string culture)
    {
        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);
    
        var subscriptionPlan = SubscriptionPlanBuilder.Build();
        factory.InjectInDatabase<short, Entity.SubscriptionsPlan>([subscriptionPlan]);
    
        var request = RequestUpdateSubscriptionPlanBuilder.Build();
        request.Id = _idEncoder.Encode(subscriptionPlan.Id + 10);
        
        var response = await DoPutAsync(request: request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var errors = await response.Content.ReadFromJsonAsync<ResponseError>();
        errors!.Errors
            .ShouldHaveSingleItem()
            .ShouldBe(
                ResourceMessagesException.ResourceManager.GetString("PLAN_NOT_FOUND", new CultureInfo(culture)));
    }
}
