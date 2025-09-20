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

    [HttpGet]
    [ProducesResponseType(typeof(ResponseRefreshToken), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh(
        [FromServices] IRefreshTokenUseCase useCase
    )
    {
        string? refreshToken = Request.Cookies[MMSConst.REFRESH_TOKEN_COOKIE_KEY];
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new ErrorOnValidationException([ResourceMessagesException.NO_REFRESH_TOKEN]);

        string? accessToken = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
        if (string.IsNullOrWhiteSpace(accessToken))
            throw new ErrorOnValidationException([ResourceMessagesException.INVALID_ACCESS_TOKEN]);
        
        var responseResult = await useCase.Execute(refreshToken: refreshToken, accessToken: accessToken);
        string? token = useCase.GetRefreshToken();
        if (string.IsNullOrWhiteSpace(token))
            Response.Cookies.Append(MMSConst.REFRESH_TOKEN_COOKIE_KEY, token, _secureTokenParam);

        return Ok(responseResult);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Logout(
        [FromServices] ILogoutUseCase useCase
    )
    {
        string? refreshToken = Request.Cookies[MMSConst.REFRESH_TOKEN_COOKIE_KEY];
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new ErrorOnValidationException([ResourceMessagesException.NO_REFRESH_TOKEN]);

        string? accessToken = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
        if (string.IsNullOrWhiteSpace(accessToken))
            throw new ErrorOnValidationException([ResourceMessagesException.INVALID_ACCESS_TOKEN]);
        
        await useCase.Execute(refreshToken: refreshToken, accessToken: accessToken);
        Response.Cookies.Delete(MMSConst.REFRESH_TOKEN_COOKIE_KEY, _secureTokenParam);

        return NoContent();
    }
}
