using System.Security.Claims;
using MMS.Domain.Security.Token;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;

namespace MMS.API.Token;

internal class HttpContextLoggedUser(
    IHttpContextAccessor httpContextAccessor
    ) : IAccessTokenClaims
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid GetUserIdentifier()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user is null)
            throw new MMSException(ResourceMessagesException.USER_CONTEXT_NOT_FOUND);

        var claim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

        if (claim is null)
            throw new MMSException(ResourceMessagesException.SID_CLAIM_NOT_FOUND);

        return Guid.Parse(claim);
    }
}
