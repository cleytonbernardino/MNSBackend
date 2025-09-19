using Microsoft.AspNetCore.Mvc;
using MMS.Application.UseCases.Auth.DoLogin;
using MMS.Application.UseCases.Auth.Logout;
using MMS.Application.UseCases.Auth.RefreshToken;
using MMS.Communication.Requests.Auth;
using MMS.Communication.Responses;
using MMS.Communication.Responses.Auth;
using MMS.Domain.ValueObjects;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;

namespace MMS.API.Controllers;

[Route("api/auth/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly CookieOptions _secureTokenParam = new()
    {
        HttpOnly = true,
        Expires = DateTime.UtcNow.AddDays(MMSConst.REFRESH_TOKEN_COOKIE_DURATION_IN_DAYS),
        Secure = true,
        SameSite = SameSiteMode.None
    };

    [HttpPost]
    [ProducesResponseType(typeof(ResponseDoLogin), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromServices] IDoLoginUseCase useCase,
        [FromBody] RequestDoLogin request
    )
    {
        var responseResult = await useCase.Execute(request);
        string token = useCase.GetRefreshToken()!;

        Response.Cookies.Append(MMSConst.REFRESH_TOKEN_COOKIE_KEY, token, _secureTokenParam);

        return Ok(responseResult);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseRefreshToken), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh(
        [FromServices] IRefreshTokenUseCase useCase,
        [FromBody] RequestRefreshToken request
    )
    {
        string? refreshToken = Request.Cookies[MMSConst.REFRESH_TOKEN_COOKIE_KEY];
        if (refreshToken is null)
            throw new ErrorOnValidationException([ResourceMessagesException.NO_REFRESH_TOKEN]);

        var responseResult = await useCase.Execute(request, refreshToken);
        string? token = useCase.GetRefreshToken();
        if (token is not null)
            Response.Cookies.Append(MMSConst.REFRESH_TOKEN_COOKIE_KEY, token, _secureTokenParam);

        return Ok(responseResult);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Logout(
        [FromServices] ILogoutUseCase useCase,
        [FromBody] RequestRefreshToken request
    )
    {
        string? refreshToken = Request.Cookies[MMSConst.REFRESH_TOKEN_COOKIE_KEY];
        if (refreshToken is null)
            throw new ErrorOnValidationException([ResourceMessagesException.NO_REFRESH_TOKEN]);

        Response.Cookies.Delete(MMSConst.REFRESH_TOKEN_COOKIE_KEY, _secureTokenParam);
        await useCase.Execute(request, refreshToken);

        return NoContent();
    }
}
