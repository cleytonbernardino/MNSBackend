using Microsoft.AspNetCore.Mvc;
using MMS.API.Binders;
using MMS.Application.UseCases.SubscriptionPlan.Register;
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
}
