using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Tokens;
using MMS.Communication.Responses;
using MMS.Domain.Enums;
using MMS.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Delete;

public class DeleteUserTest(CustomWebApplicationFactory factory) : MmsClassFixture(factory)
{
    protected override string Method => "api/user";
    
    private readonly IdEncoderForTests _idEncoder = new();
    
    [Fact]
    public async Task Success()
    {
        var userToDelete = factory.InjectInDatabase(UserBuilder.Build());

        string id = _idEncoder.Encode(userToDelete.Id);
        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.ManagerUser.UserIdentifier, UserRolesEnum.MANAGER);
        
        var response = await DoDeleteAsync(parameter: id, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Without_Permission(string culture)
    {
        string id = _idEncoder.Encode(factory.AdminUser.Id);
        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.EmployeeUser.UserIdentifier, UserRolesEnum.MANAGER);
        
        var response = await DoDeleteAsync(parameter: id, token: token, culture: culture);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        
        var errors = await response.Content.ReadFromJsonAsync<ResponseError>();
        errors!.Errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ResourceManager.GetString("NO_PERMISSION", new CultureInfo(culture)));
    }

    [Fact]
    public async Task Error_Authorize_Denies_Invalid_Roles()
    {
        string id = _idEncoder.Encode(factory.EmployeeUser.Id);
        string token = JwtTokenGeneratorBuilder.Build()
            .Generate(factory.ManagerUser.UserIdentifier, UserRolesEnum.EMPLOYEE);
        
        var response = await DoDeleteAsync(parameter: id, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
