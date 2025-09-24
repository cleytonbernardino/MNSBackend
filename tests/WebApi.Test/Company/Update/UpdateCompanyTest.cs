using CommonTestUtilities.Cryptography;
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

namespace WebApi.Test.Company.Update;

public class UpdateCompanyTest(CustomWebApplicationFactory factory): MmsClassFixture(factory)
{
    protected override string Method => "api/admin/company";

    private readonly IdEncoderForTests _idEncoder = new();

    [Fact]
    public async Task Success()
    {
        var companies = factory.RegisterCompaniesAndGetCompanies();
        
        string token = JwtTokenGeneratorBuilder.Build().Generate(
            factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN
            );
        var request = RequestUpdateCompanyBuilder.Build();

        string id = _idEncoder.Encode(companies.Last().Id);
        string url = $"{Method}/{id}";
        
        var response = await DoPutAsync(request, token, customUrl: url);
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_No_Permission(string culture)
    {
        string token = JwtTokenGeneratorBuilder.Build().Generate(
            factory.ManagerUser.UserIdentifier, UserRolesEnum.ADMIN
        );
        var request = RequestUpdateCompanyBuilder.Build();

        string id = _idEncoder.Encode(1);
        string url = $"{Method}/{id}";
        
        var response = await DoPutAsync(request, token, culture: culture, customUrl: url);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        var errors = await response.Content.ReadFromJsonAsync<ResponseError>();
        errors!.Errors
            .ShouldHaveSingleItem()
            .ShouldBe(
                ResourceMessagesException.ResourceManager.GetString("NO_PERMISSION", new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Validator(string culture)
    {
        string token = JwtTokenGeneratorBuilder.Build().Generate(
            factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN
        );
        var request = RequestUpdateCompanyBuilder.Build();
        request.DoingBusinessAs = new string('A', 200);

        string id = _idEncoder.Encode(1);
        string url = $"{Method}/{id}";
        
        var response = await DoPutAsync(request, token, culture: culture, customUrl: url);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await response.Content.ReadFromJsonAsync<ResponseError>();
        errors!.Errors
            .ShouldHaveSingleItem()
            .ShouldBe(
                ResourceMessagesException.ResourceManager.GetString("DBA_MAX_LENGTH", new CultureInfo(culture)));
    }
}
