using CommonTestUtilities.Tokens;
using MMS.Communication.Responses.User;
using Shouldly;
using System.Net;
using MMS.Domain.Enums;
using System.Net.Http.Json;

namespace WebApi.Test.Company.ListUsers;

public class ListCompanyUsers(CustomWebApplicationFactory factory) : MmsClassFixture(factory)
{
    protected override string Method => "api/company/users";

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);

        var response = await DoGetAsync(token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<ResponseListShortUsers>();
        result!.Users
            .Count()
            .ShouldBe(factory.UserInDataBase);
    }

    [Fact]
    public async Task Error_User_Without_Permission()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.ManagerUser.UserIdentifier, UserRolesEnum.EMPLOYEE);

        var response = await DoGetAsync(token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

}
