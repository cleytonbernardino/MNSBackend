using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Tokens;
using MMS.Communication.Responses;
using MMS.Communication.Responses.Company;
using MMS.Domain.Enums;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Company.Get;

public class GetCompanyTest(CustomWebApplicationFactory factory): MmsClassFixture(factory)
{
    protected override string Method => "api/company";

    private readonly IdEncoderForTests _idEncoder = new();
    
    [Fact]
    public async Task Success()
    {
        var companiesList = CompanyBuilder.BuildInBatch(factory.ManagerUser);
        var companies = factory.RegisterCompaniesAndGetCompanies(companiesList);
        
        string id = _idEncoder.Encode(companies.Last().Id);
        string url = $"{Method}/{id}";

        string token = JwtTokenGeneratorBuilder.Build().Generate(
            factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER
        );

        var response = await DoGetAsync(token: token, customUrl: url);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<ResponseCompany>();
        result!.LegalName.ShouldBe(companies.Last().LegalName);
        result!.Manager.ShouldBe(factory.ManagerUser.FirstName);
    }
    
    [Fact]
    public async Task Success_Admin_ByPass()
    {
        var companies = factory.RegisterCompaniesAndGetCompanies();
        
        string id = _idEncoder.Encode(companies.Last().Id);
        string url = $"{Method}/{id}";

        string token = JwtTokenGeneratorBuilder.Build().Generate(
            factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN
        );

        var response = await DoGetAsync(token: token, customUrl: url);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<ResponseCompany>();
        result!.LegalName.ShouldBe(companies.Last().LegalName);
        result!.Manager.ShouldBe(companies.Last().Manager!.FirstName);
    }
    
    [Fact]
    public async Task Error_User_No_Has_Permission()
    {
        var companies = factory.RegisterCompaniesAndGetCompanies();
        
        string id = _idEncoder.Encode(companies.Last().Id);
        string url = $"{Method}/{id}";

        string token = JwtTokenGeneratorBuilder.Build().Generate(
            factory.RHUser.UserIdentifier, UserRolesEnum.RH
        );

        var response = await DoGetAsync(token: token, customUrl: url);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_User_No_Has_Permission_Use_Case(string culture)
    {
        var companies = factory.RegisterCompaniesAndGetCompanies();
        
        string id = _idEncoder.Encode(companies.Last().Id);
        string url = $"{Method}/{id}";

        string token = JwtTokenGeneratorBuilder.Build().Generate(
            factory.RHUser.UserIdentifier, UserRolesEnum.ADMIN
        );

        var response = await DoGetAsync(token: token, culture: culture, customUrl: url);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        
        var error = await response.Content.ReadFromJsonAsync<ResponseError>();
        error!.Errors
            .ShouldHaveSingleItem()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("NO_PERMISSION", new CultureInfo(culture)));
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Company_Not_Found(string culture)
    {
        var companies = factory.RegisterCompaniesAndGetCompanies();
        
        string id = _idEncoder.Encode(companies.Last().Id + 1);
        string url = $"{Method}/{id}";

        string token = JwtTokenGeneratorBuilder.Build().Generate(
            factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER
        );

        var response = await DoGetAsync(token: token, culture: culture, customUrl: url);
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        
        var error = await response.Content.ReadFromJsonAsync<ResponseError>();
        error!.Errors
            .ShouldHaveSingleItem()
            .ShouldBe(
                ResourceMessagesException.ResourceManager.GetString("COMPANY_NOT_FOUND", new CultureInfo(culture))
                );
    }
}
