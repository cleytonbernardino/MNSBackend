using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MMS.Communication.Requests.Company;
using MMS.Communication.Responses;
using MMS.Domain.Enums;
using MMS.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using WebApi.Test.InlineData;
using Entity = MMS.Domain.Entities;

namespace WebApi.Test.CompanySubscription.Register;

public class RegisterCompanySubscriptionTest(CustomWebApplicationFactory factory): MmsClassFixture(factory)
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
            IsBillingAnnual = false
        };
    
        var response = await DoPostAsync(request: request, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }
    
    [Fact]
    public async Task Error_No_Permission()
    {
        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);

        var request = RequestRegisterCompanySubscriptionBuilder.Build();
    
        var response = await DoPostAsync(request: request, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Validator(string culture)
    {
        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);

        var request = RequestRegisterCompanySubscriptionBuilder.Build();
        request.SubscriptionId = string.Empty;
    
        var response = await DoPostAsync(request: request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await response.Content.ReadFromJsonAsync<ResponseError>();
        errors!.Errors.
            ShouldHaveSingleItem()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("EMPTY_SUBSCRIPTION_ID", new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Company_Not_Found(string culture)
    {
        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);

        var request = RequestRegisterCompanySubscriptionBuilder.Build();
        request.CompanyId = _idEncoder.Encode(2000);
    
        var response = await DoPostAsync(request: request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var errors = await response.Content.ReadFromJsonAsync<ResponseError>();
        errors!.Errors.
            ShouldHaveSingleItem()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("COMPANY_NOT_FOUND", new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Subscription_Plan_Not_Found(string culture)
    {
        var company = CompanyBuilder.Build();
        factory.InjectInDatabase(company);
        
        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);

        var request = new RequestRegisterCompanySubscription
        {
            CompanyId = _idEncoder.Encode(company.Id),
            SubscriptionId = _idEncoder.Encode(600),
            IsBillingAnnual = false
        };
    
        var response = await DoPostAsync(request: request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var errors = await response.Content.ReadFromJsonAsync<ResponseError>();
        errors!.Errors.
            ShouldHaveSingleItem()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("SUBSCRIPTION_DOES_NOT_EXIST", new CultureInfo(culture)));
    }
}
