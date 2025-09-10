using MMS.Domain.Security.Token;

namespace MMS.API.Token;

internal class HttpContextTokenValue(
    IHttpContextAccessor httpContextAccessor
    ) : ITokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string Value()
    {
        string authorization = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();
        return authorization["Bearer".Length..];
    }
}
