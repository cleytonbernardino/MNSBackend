using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using MMS.Communication.Responses;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
#if !DEBUG
using MMS.Exceptions;
#endif

namespace MMS.API.Filter;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is MMSException)
        {
            HandleProjectException(context);
        }
        else if (context.Exception is SecurityTokenValidationException)
        {
            HandleTokenException(context);
        }
        else
        {
            ThrowUnknowException(context);
        }
    }

    private static void HandleProjectException(ExceptionContext context)
    {
        ResponseError responseError = new();
        switch (context.Exception)
        {
            case NoPermissionException  or InvalidLoginException:
                responseError.Errors.Add(context.Exception.Message);
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Result = new UnauthorizedObjectResult(responseError);
                break;
            case ErrorOnValidationException exception:
                responseError.Errors.AddRange(exception.ErrorMessages);
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Result = new BadRequestObjectResult(responseError);
                break;
            case NotFoundException notFoundException:
                responseError.Errors.Add(notFoundException.Message);
                context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Result = new NotFoundObjectResult(responseError);
                break;
        }
    }

    private static void HandleTokenException(ExceptionContext context)
    {
        ResponseError responseError = new();
        switch (context.Exception)
        {
            case SecurityTokenExpiredException:
                responseError.Errors.Add(ResourceMessagesException.EXPIRED_TOKEN);
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Result = new UnauthorizedObjectResult(responseError);
                break;
        }
    }
    
    private static void ThrowUnknowException(ExceptionContext context)
    {
        var responseError = new ResponseError().Errors;
#if DEBUG
        responseError.Add(context.Exception.Message);
        context.Result = new ObjectResult(responseError);
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
#else
        responseError.Add(ResourceMessagesException.UNKNOWN_ERROR);
        context.Result = new ObjectResult(new ResponseError());
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
#endif
    }
}
