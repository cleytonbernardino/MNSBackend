using Microsoft.AspNetCore.Mvc;
using MMS.Application.UseCases.Company.List;
using MMS.Application.UseCases.Company.Register;
using MMS.Communication;

namespace MMS.API.ControllersAdmin;

public class CompanyController : MmsAdminBaseController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromBody] RequestRegisterCompany request,
        [FromServices] IRegisterCompanyUseCase useCase
        )
    {
        await useCase.Execute(request);
        return Created("", null);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseShortCompanies), StatusCodes.Status200OK)]
    public IActionResult ListCompanies(
        [FromServices] IListCompaniesUseCase useCase
        )
    {
        var response = useCase.Execute();
        return Ok(response);
    }
}
