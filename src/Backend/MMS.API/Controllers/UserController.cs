using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MMS.API.Binders;
using MMS.Application.UseCases.User.Delete;
using MMS.Application.UseCases.User.Register;
using MMS.Application.UseCases.User.Update;
using MMS.Application.UseCases.User.Update.Password;
using MMS.Communication.Requests.User;
using MMS.Communication.Responses;

namespace MMS.API.Controllers;

public class UserController : MMSBaseController
{
    [HttpPost]
    [Authorize(Roles = "ADMIN,RH,SUB_MANAGER,MANAGER")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Register(
        [FromBody] RequestRegisterUser request,
        [FromServices] IRegisterUserUseCase useCase
    )
    {
        await useCase.Execute(request);
        return Created("", null);
    }

    [HttpPut]
    [Authorize(Roles = "ADMIN,RH,SUB_MANAGER,MANAGER")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        [FromBody] RequestUpdateUser request,
        [FromServices] IUpdateUserUseCase useCase
    )
    {
        await useCase.Execute(request);
        return NoContent();
    }

    [HttpPut("change-password")]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdatePassword(
        [FromBody] RequestUpdateUserPassword request,
        [FromServices] IUpdateUserPasswordUseCase useCase
    )
    {
        await useCase.Execute(request);
        return NoContent();
    }

    [HttpDelete]
    [Authorize(Roles = "ADMIN,RH,SUB_MANAGER,MANAGER")]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(
        [FromRoute] [ModelBinder(typeof(MmsIdBinder))]
        long id,
        [FromServices] IDeleteUserUseCase useCase
    )
    {
        await useCase.Execute(id);
        return NoContent();
    }
}
