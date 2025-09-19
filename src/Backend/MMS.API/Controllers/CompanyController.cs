using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMS.Application.UseCases.Company.ListUsers;
using MMS.Communication.Responses.User;

namespace MMS.API.Controllers;

[Authorize(Roles = "ADMIN,RH,SUB_MANAGER,MANAGER")]
public class CompanyController : MMSBaseController
{
    [HttpGet("Users")]
    [ProducesResponseType(typeof(ResponseListShortUsers), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListUsers(
        [FromServices] IListCompanyUsersUseCase useCase
    )
    {
        var response = await useCase.Execute();
        return Ok(response);
    }
}
