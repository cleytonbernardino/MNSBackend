using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Tokens;
using MMS.Communication.Requests.Company;
using MMS.Domain.Enums;
using Shouldly;
using System.Net;
using Entity = MMS.Domain.Entities;

namespace WebApi.Test.CompanySubscription.Update;

public class UpdateCompanySubscription(CustomWebApplicationFactory factory): MmsClassFixture(factory)
{
    protected override string Method => "api/admin/company/plan";

    private readonly IdEncoderForTests _idEncoder = new();

    [Fact]
    public async Task Success()
    {
        var company = CompanyBuilder.Build();
        factory.InjectInDatabase(company);

        var subscriptionPlan = SubscriptionPlanBuilder.Build();
        factory.InjectInDatabase<short, Entity.SubscriptionsPlan>([subscriptionPlan]);

        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);

        var request = new RequestRegisterCompanySubscription
        {
            CompanyId = _idEncoder.Encode(company.Id),
            SubscriptionId = _idEncoder.Encode(subscriptionPlan.Id),
            IsBillingAnnual = true
        };
        
        var response = await DoPutAsync(request: request, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Success_Company_Already_Has_Plan()
    {
        var subscriptionPlans = SubscriptionPlanBuilder.BuildInBatch(count: 2);
        factory.InjectInDatabase<short, Entity.SubscriptionsPlan>(subscriptionPlans);

        var company = CompanyBuilder.Build();
        factory.InjectInDatabase(company);

        company.CompanySubscription = new Entity.CompanySubscription
        {
            Id = 0,
            Active = true,
            CompanyId = company.Id,
            IsBillingAnnual = false,
            NextBillingDate = DateTime.Today,
            PaymentMethod = 0,
            PaymentStatus = 0,
            SubscriptionPlan = subscriptionPlans[0]
        };
        factory.UpdateInDataBase(company);

        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);

        var request = new RequestRegisterCompanySubscription
        {
            CompanyId = _idEncoder.Encode(company.Id),
            SubscriptionId = _idEncoder.Encode(subscriptionPlans[1].Id),
            IsBillingAnnual = true
        };
    
        var response = await DoPutAsync(request: request, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}
