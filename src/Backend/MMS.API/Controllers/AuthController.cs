using Microsoft.AspNetCore.Mvc;
using MMS.Application.UseCases.Auth.RefreshToken;
using MMS.Application.UseCases.Login.DoLogin;
using MMS.Communication;
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
    [ProducesResponseType(typeof(ResponseRegisteredUser), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromServices] IDoLoginUseCase useCase,
        [FromBody] RequestLogin request
        )
    {
        var responseResult = await useCase.Execute(request);
        
        Response.Cookies.Append(MMSConst.REFRESH_TOKEN_COOKIE_KEY, responseResult.Tokens.RefreshToken, _secureTokenParam);
        responseResult.Tokens.RefreshToken = string.Empty;
        
        return Ok(responseResult);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ResponseToken), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh(
        [FromServices] IRefreshTokenUseCase useCase,
        [FromBody] RequestRefreshAccessToken request
        )
    {
        string? refreshToken = Request.Cookies[MMSConst.REFRESH_TOKEN_COOKIE_KEY];
        if (refreshToken is null)
            throw new ErrorOnValidationException([ResourceMessagesException.NO_REFRESH_TOKEN]);
        
        var responseResult = await useCase.Execute(request, refreshToken);
        Response.Cookies.Append(MMSConst.REFRESH_TOKEN_COOKIE_KEY, responseResult.RefreshToken, _secureTokenParam);
        
        return Ok(responseResult);
    }
        )
    {
        var response = await useCase.Execute(request);
        return Ok(response);
    }
}
