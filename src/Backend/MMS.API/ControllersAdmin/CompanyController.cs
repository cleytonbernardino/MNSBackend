using Microsoft.AspNetCore.Mvc;
using MMS.API.Binders;
using MMS.Application.UseCases.Company.Delete;
using MMS.Application.UseCases.Company.List;
using MMS.Application.UseCases.Company.Register;
using MMS.Application.UseCases.Company.Update;
using MMS.Application.UseCases.CompanySubscription.RegisterAndUpdate;
using MMS.Communication.Requests.Company;
using MMS.Communication.Responses;
using MMS.Communication.Responses.Company;

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

    [HttpPost("plan")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RegisterPlan(
        [FromBody] RequestRegisterCompanySubscription request,
        [FromServices] IRegisterCompanySubscriptionUseCase useCase
        )
    {
        await useCase.Execute(request);
        return Created("", null);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseShortCompanies), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListCompanies(
        [FromServices] IListCompaniesUseCase useCase
    )
    {
        var response = await useCase.Execute();
        return Ok(response);
    }

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] [ModelBinder(typeof(MmsIdBinder))] long id,
        [FromBody] RequestUpdateCompany request,
        [FromServices] IUpdateCompanyUseCase useCase
        )
    {
        await useCase.Execute(request, id);
        return NoContent();
    }

    [HttpPut("plan")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdatePlan(
        [FromBody] RequestRegisterCompanySubscription request,
        [FromServices] IRegisterCompanySubscriptionUseCase useCase
    )
    {
        await useCase.Execute(request);
        return NoContent();
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(
        [FromRoute] [ModelBinder(typeof(MmsIdBinder))] long id,
        [FromServices] IDeleteCompanyUseCase useCase
        )
    {
        await useCase.Execute(id);
        return NoContent();
    }
}
