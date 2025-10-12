using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Tokens;
using MMS.Domain.Enums;
using Shouldly;
using System.Net;
using Entity = MMS.Domain.Entities;

namespace WebApi.Test.Company.Delete;

public class DeleteCompanyTest(CustomWebApplicationFactory factory): MmsClassFixture(factory)
{
    protected override string Method => "api/admin/company";

    private readonly IdEncoderForTests _idEncoder = new();
    
    [Fact]
    public async Task Success()
    {
        var company = factory.InjectInDatabase(CompanyBuilder.Build());
        string token = JwtTokenGeneratorBuilder.Build().Generate(
            factory.AdminUser.UserIdentifier, UserRolesEnum.ADMIN
        );
        string id = _idEncoder.Encode(company.Id); 
        
        var response = await DoDeleteAsync(parameter: id, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Error_Delete_Companies_Without_Being_Admin()
    {
        string token = JwtTokenGeneratorBuilder.Build().Generate(
            factory.ManagerUser.UserIdentifier, UserRolesEnum.ADMIN
        );
        string id = _idEncoder.Encode(3); 
        
        var response = await DoDeleteAsync(parameter: id, token: token);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
