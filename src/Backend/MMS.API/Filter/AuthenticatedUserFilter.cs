using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using MMS.Communication.Responses;
using MMS.Domain.Repositories.User;
using MMS.Domain.Security.Token;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;

namespace MMS.API.Filter;

public class AuthenticatedUserFilter(
    IAccessTokenClaims accessTokenValidator,
    IUserReadOnlyRepository repository
    ) : IAsyncAuthorizationFilter
{
    private readonly IAccessTokenClaims _accessTokenValidator = accessTokenValidator;
    private readonly IUserReadOnlyRepository _repository = repository;

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            ValidTokenOnRequest(context);

            Guid userIdentifier = _accessTokenValidator.GetUserIdentifier();

            bool exist = await _repository.ExistActiveUserWithIdentifier(userIdentifier);

            if (!exist)
                throw new MMSException(ResourceMessagesException.USER_DOES_NOT_HAVE_PERMISSION);
        } 
        catch (SecurityTokenExpiredException)
        {
            var responseError = new ResponseError().Errors;
            responseError.Add(ResourceMessagesException.EXPIRED_TOKEN);
            context.Result = new UnauthorizedObjectResult(responseError);
        } 
        catch (MMSException ex)
        {
            var responseError = new ResponseError().Errors;
            responseError.Add(ex.Message);
            context.Result = new UnauthorizedObjectResult(responseError);
        } 
        catch (Exception)
        {
            var responseError = new ResponseError().Errors;
            responseError.Add(ResourceMessagesException.USER_DOES_NOT_HAVE_PERMISSION);
            context.Result = new UnauthorizedObjectResult(responseError);
        }
    }

    private static void ValidTokenOnRequest(AuthorizationFilterContext context)
    {
        string? authorization = context.HttpContext.Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authorization))
            throw new MMSException(ResourceMessagesException.NO_TOKEN);
    }
}
