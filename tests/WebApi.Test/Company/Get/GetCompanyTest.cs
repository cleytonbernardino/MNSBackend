using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Tokens;
using MMS.Communication.Responses;
using MMS.Communication.Responses.Company;
using MMS.Domain.Enums;
using MMS.Exceptions;
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
        var companies = CompanyBuilder.BuildInBatch(manager:factory.ManagerUser);
        factory.InjectInDatabase(companies);
        
        string id = _idEncoder.Encode(companies[0].Id);
        string token = JwtTokenGeneratorBuilder.Build().Generate(
            factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER
        );

        var response = await DoGetAsync(token: token, parameter: id);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<ResponseCompany>();
        result!.LegalName.ShouldBe(companies[0].LegalName);
        result!.Manager.ShouldBe(factory.ManagerUser.FirstName);
    }
    
    [Fact]
    public async Task Success_Admin_ByPass()
    {
        var company = CompanyBuilder.Build(manager: factory.RHUser);
        factory.InjectInDatabase(company);

        string id = _idEncoder.Encode(company.Id);
        string token = JwtTokenGeneratorBuilder.Build().Generate(
            factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN
        );

        var response = await DoGetAsync(token: token, parameter: id);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<ResponseCompany>();
        result!.LegalName.ShouldBe(company.LegalName);
        result!.Manager.ShouldBe(factory.RHUser.FirstName);
    }
    
    [Fact]
    public async Task Error_User_No_Has_Permission()
    {
        string token = JwtTokenGeneratorBuilder.Build().Generate(
            factory.RHUser.UserIdentifier, UserRolesEnum.RH
        );

        var response = await DoGetAsync(token: token, parameter: "yyy");
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Company_Not_Found(string culture)
    {
        string token = JwtTokenGeneratorBuilder.Build().Generate(
            factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER
        );

        var response = await DoGetAsync(token: token, culture: culture, parameter: "invalidID");
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        
        var error = await response.Content.ReadFromJsonAsync<ResponseError>();
        error!.Errors
            .ShouldHaveSingleItem()
            .ShouldBe(
                ResourceMessagesException.ResourceManager.GetString("COMPANY_NOT_FOUND", new CultureInfo(culture))
                );
    }
}
