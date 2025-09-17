using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;
using MMS.Domain.Enums;

namespace WebApi.Test.Company.ListUsers;

public class ListCompanyUsers(CustomWebApplicationFactory factory) : MmsClassFixture(factory)
{
    private const string METHOD = "api/company/users";

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);

        var response = await DoGetAsync(method: METHOD, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var rootElement = await GetRootElement(response);
        var responseObject = rootElement.GetProperty("users").EnumerateArray();

        responseObject
            .Count()
            .ShouldBe(factory.UserInDataBase);
    }

    [Fact]
    public async Task Error_User_Without_Permission()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(factory.ManagerUser.UserIdentifier, UserRolesEnum.EMPLOYEE);

        var response = await DoGetAsync(method: METHOD, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

}
