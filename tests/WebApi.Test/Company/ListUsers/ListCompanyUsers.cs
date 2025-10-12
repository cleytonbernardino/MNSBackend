using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Tokens;
using MMS.Communication.Responses.User;
using MMS.Domain.Enums;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace WebApi.Test.Company.ListUsers;

public class ListCompanyUsers(CustomWebApplicationFactory factory) : MmsClassFixture(factory)
{
    protected override string Method => "api/company/users";

    private readonly IdEncoderForTests _idEncoder = new();

    [Fact]
    public async Task Success()
    {
        var company = CompanyBuilder.Build();
        factory.InjectInDatabase(company);

        var users = UserBuilder.BuildInBatch(2);
        users[0].Role = UserRolesEnum.MANAGER;
        foreach (var user in users)
        {
            user.CompanyId = company.Id;
        }
        factory.InjectInDatabase(users);

        string id = _idEncoder.Encode(company.Id);
        var token = JwtTokenGeneratorBuilder.Build()
            .Generate(users[0].UserIdentifier, UserRolesEnum.MANAGER);

        var response = await DoGetAsync(token: token, parameter: id);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<ResponseListShortUsers>();
        result!.Users
            .Count()
            .ShouldBe(users.Length);
        result.Users.First().FirstName.ShouldBe(users[0].FirstName);
    }

    [Fact]
    public async Task Success_Admin_Ignores_Validation()
    {
        var company = CompanyBuilder.Build();
        factory.InjectInDatabase(company);

        var users = UserBuilder.BuildInBatch(2);
        foreach (var user in users)
        {
            user.CompanyId = company.Id;
        }
        factory.InjectInDatabase(users);

        string id = _idEncoder.Encode(company.Id);
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN);

        var response = await DoGetAsync(token: token, parameter: id);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<ResponseListShortUsers>();
        result!.Users
            .Count()
            .ShouldBe(users.Length);
        result.Users.First().FirstName.ShouldBe(users[0].FirstName);
    }
    
    [Fact]
    public async Task Error_User_Without_Permission()
    {
        var token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.EmployeeUser.UserIdentifier, UserRolesEnum.EMPLOYEE);

        var response = await DoGetAsync(token: token, parameter: "yyy");
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

}
