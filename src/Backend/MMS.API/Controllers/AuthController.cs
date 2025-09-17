using Microsoft.AspNetCore.Mvc;
using MMS.Application.UseCases.Auth.RefreshToken;
using MMS.Application.UseCases.Login.DoLogin;
using MMS.Communication;

namespace MMS.API.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(ResponseRegisteredUser), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromServices] IDoLoginUseCase useCase,
        [FromBody] RequestLogin request
        )
    {
        var response = await useCase.Execute(request);
        return Ok(response);
    }
    
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(ResponseToken), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAccessToken(
        [FromServices] IRefreshTokenUseCase useCase,
        [FromBody] RequestRefreshToken request
        )
    {
        var response = await useCase.Execute(request);
        return Ok(response);
    }
}
