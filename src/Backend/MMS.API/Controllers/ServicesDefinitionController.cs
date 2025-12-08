using Microsoft.AspNetCore.Mvc;
using MMS.API.Binders;
using MMS.Application.UseCases.ServiceDefinition.Delete;
using MMS.Application.UseCases.ServiceDefinition.Get;
using MMS.Application.UseCases.ServiceDefinition.Register;
using MMS.Application.UseCases.ServiceDefinition.Update;
using MMS.Communication.Requests.ServiceDefinition;
using MMS.Communication.Responses;
using MMS.Communication.Responses.ServiceDefinition;

namespace MMS.API.Controllers;

[Route("api/services")]
public class ServicesDefinitionController: MMSBaseController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterServicesUseCase useCase,
        [FromBody] RequestRegisterServices request)
    {
        await useCase.Execute(request);
        return Created("", null);
    }
    
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseGetServiceDefinition), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Detail(
        [FromServices] IGetServiceDefinitionUseCase useCase,
        [FromRoute] [ModelBinder(typeof(MmsIdBinder))] long id)
    {
        var result = await useCase.Execute(id);
        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
        [FromServices] IUpdateServicesDefinitionUseCase useCase,
        [FromBody] RequestUpdateServiceDefinition request)
    {
        await useCase.Execute(request);
        return NoContent();
    }
    
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Route("{id}")]
    public async Task<IActionResult> Delete(
        [FromServices] IDeleteServiceDefinitionUseCase useCase,
        [FromRoute] [ModelBinder(typeof(MmsIdBinder))] long id )
    {
        await useCase.Execute(id);
        return NoContent();
    }
}
