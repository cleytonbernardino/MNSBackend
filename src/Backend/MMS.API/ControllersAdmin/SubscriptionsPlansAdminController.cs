using Microsoft.AspNetCore.Mvc;
using MMS.API.Binders;
using MMS.Application.UseCases.SubscriptionPlan.Delete;
using MMS.Application.UseCases.SubscriptionPlan.Register;
using MMS.Application.UseCases.SubscriptionPlan.Update;
using MMS.Communication.Requests.SubscriptionsPlans;
using MMS.Communication.Responses;

namespace MMS.API.ControllersAdmin;

[Route("api/admin/plans")]
public class SubscriptionsPlansAdminController : MmsAdminBaseController
{
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromBody] RequestRegisterSubscriptionPlan request,
        [FromServices] IRegisterSubscriptionPlanUseCase useCase
        )
    {
        await useCase.Execute(request);
        return Created("", null);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromBody] RequestUpdateSubscriptionPlan request,
        [FromServices] IUpdateSubscriptionPlanUseCase useCase
        )
    {
        await useCase.Execute(request);
        return NoContent();
    }
    
    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] [ModelBinder(typeof(MmsIdBinder))] long id,
        [FromServices] IDeleteSubscriptionPlanUseCase useCase
        )
    {
        bool idValid = short.TryParse(id.ToString(), out short shortId);
        if (!idValid)
            return NotFound();
    
        await useCase.Execute(shortId);
        return NoContent();
    }
}
