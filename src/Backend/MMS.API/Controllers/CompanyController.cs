using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMS.API.Binders;
using MMS.Application.UseCases.Company.Get;
using MMS.Application.UseCases.Company.ListUsers;
using MMS.Communication.Responses.Company;
using MMS.Communication.Responses.User;

namespace MMS.API.Controllers;

public class CompanyController : MMSBaseController
{
    [HttpGet("Users")]
    [ProducesResponseType(typeof(ResponseListShortUsers), StatusCodes.Status200OK)]
    [Authorize(Roles = "ADMIN,RH,SUB_MANAGER,MANAGER")]
    public async Task<IActionResult> ListUsers(
        [FromServices] IListCompanyUsersUseCase useCase
    )
    {
        var response = await useCase.Execute();
        return Ok(response);
    }
    
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseCompany), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "ADMIN, MANAGER")]
    public async Task<IActionResult> GetDetail(
        [FromServices] IGetCompanyUseCase useCase,
        [FromRoute] [ModelBinder(typeof(MmsIdBinder))] long id
    )
    {
        var response = await useCase.Execute(id);
        return Ok(response);
    }
}
