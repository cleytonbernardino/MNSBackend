using CommonTestUtilities.Entities;
using MMS.Communication.Responses.SubscriptionsPlans;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using Entity = MMS.Domain.Entities;

namespace WebApi.Test.SubscriptionPlan.List;

public class SubscriptionPlansListTest(CustomWebApplicationFactory factory): MmsClassFixture(factory)
{
    protected override string Method => "api/plans/list";

    [Fact]
    public async Task Success()
    {
        var plans = SubscriptionPlanBuilder.BuildInBatch();
        factory.InjectInDatabase<short, Entity.SubscriptionsPlan>(plans);
        
        var response = await DoGetAsync();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<ResponseListSubscriptionPlans>();
        result!.SubscriptionPlans.Count.ShouldBe(plans.Length);
    }
}
