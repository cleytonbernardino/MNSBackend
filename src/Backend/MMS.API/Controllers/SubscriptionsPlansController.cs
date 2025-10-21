using Microsoft.AspNetCore.Mvc;
using MMS.Application.UseCases.SubscriptionPlan.List;
using MMS.Communication.Responses.SubscriptionsPlans;

namespace MMS.API.Controllers;

[Route("api/plans/[action]")]
[ApiController]
public class SubscriptionsPlansController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseSubscriptionPlan), StatusCodes.Status200OK)]
    public async Task<IActionResult> List([FromServices] IListSubscriptionPlanUseCase useCase)
    {
        var result = await useCase.Execute();
        return Ok(result);
    }
}
