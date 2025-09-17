using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MMS.Domain.Enums;
using MMS.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using WebApi.Test.InlineData;

namespace WebApi.Test.Company.Register;

public class RegisterCompanyTest(CustomWebApplicationFactory factory) : MmsClassFixture(factory)
{
    private const string METHOD = "api/admin/Company";

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);

        var request = RequestRegisterCompanyBuilder.Build();

        var response  = await DoPostAsync(method: METHOD, request: request, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Success_Without_DBA()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);

        var request = RequestRegisterCompanyBuilder.Build();
        request.DoingBusinessAs = string.Empty;

        var response = await DoPostAsync(method: METHOD, request: request, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Success_Without_Business_Email()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);

        var request = RequestRegisterCompanyBuilder.Build();
        request.BusinessEmail = string.Empty;

        var response = await DoPostAsync(method: METHOD, request: request, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Success_Without_WebSite()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);

        var request = RequestRegisterCompanyBuilder.Build();
        request.Website = string.Empty;

        var response = await DoPostAsync(method: METHOD, request: request, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_User_Without_Permission_To_Register(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.EmployeeUser.UserIdentifier, UserRolesEnum.EMPLOYEE);

        var request = RequestRegisterCompanyBuilder.Build();

        var response = await DoPostAsync(METHOD, request, token, culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_CNPJ_Not_Valid(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);

        var request = RequestRegisterCompanyBuilder.Build();
        request.CNJP = "750068710001";

        var response = await DoPostAsync(method: METHOD, request: request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await GetArrayFromResponse(response);
        errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("CNPJ_INVALID", new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_CNPJ_Empty(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);

        var request = RequestRegisterCompanyBuilder.Build();
        request.CNJP = string.Empty;

        var response = await DoPostAsync(method: METHOD, request: request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await GetArrayFromResponse(response);
        errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("CNPJ_EMPTY", new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Legal_Name_Empty(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);

        var request = RequestRegisterCompanyBuilder.Build();
        request.LegalName = string.Empty;

        var response = await DoPostAsync(method: METHOD, request: request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await GetArrayFromResponse(response);
        errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("LEGAL_NAME_EMPTY", new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_CEP_Not_Valid(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);

        var request = RequestRegisterCompanyBuilder.Build();
        request.CEP = "0814173";

        var response = await DoPostAsync(method: METHOD, request: request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await GetArrayFromResponse(response);
        errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("CEP_INVALID", new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_CEP_Empty(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);

        var request = RequestRegisterCompanyBuilder.Build();
        request.CEP = string.Empty;

        var response = await DoPostAsync(method: METHOD, request: request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await GetArrayFromResponse(response);
        errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("CEP_EMPTY", new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Address_Number_Empty(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);

        var request = RequestRegisterCompanyBuilder.Build();
        request.AddressNumber = string.Empty;

        var response = await DoPostAsync(method: METHOD, request: request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await GetArrayFromResponse(response);
        errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("ADDRESS_NUMBER_EMPTY", new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Phone_Number_Empty(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);

        var request = RequestRegisterCompanyBuilder.Build();
        request.PhoneNumber = string.Empty;

        var response = await DoPostAsync(method: METHOD, request: request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await GetArrayFromResponse(response);
        errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("PHONE_EMPTY", new CultureInfo(culture)));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Phone_Number_Invalid(string culture)
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);

        var request = RequestRegisterCompanyBuilder.Build();
        request.PhoneNumber = "11 9873-1345";

        var response = await DoPostAsync(method: METHOD, request: request, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errors = await GetArrayFromResponse(response);
        errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("PHONE_NOT_VALID", new CultureInfo(culture)));
    }
}
