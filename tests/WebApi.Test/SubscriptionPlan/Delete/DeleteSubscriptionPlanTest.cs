using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Tokens;
using MMS.Domain.Enums;
using Shouldly;
using System.Net;
using Entity = MMS.Domain.Entities;

namespace WebApi.Test.SubscriptionPlan.Delete;

public class DeleteSubscriptionPlanUseCaseTest(CustomWebApplicationFactory factory): MmsClassFixture(factory)
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
        
        string id = _idEncoder.Encode(subscriptionPlan.Id);
        
        var response = await DoDeleteAsync(token: token, parameter: id);
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task Error_No_Permission()
    {
        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);
        
        var response = await DoDeleteAsync(token: token, parameter: "yyy");
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
