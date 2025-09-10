using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MMS.Communication;
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
        }else
        {
            ThrowUnknowException(context);
        }
    }

    private static void HandleProjectException(ExceptionContext context)
    {
        ResponseError responseError = new();
        var errorKey = responseError.Errors;
        switch (context.Exception)
        {
            case NoPermissionException or InvalidLoginException:
                errorKey.Add(context.Exception.Message);
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Result = new UnauthorizedObjectResult(responseError);
                break;
            case ErrorOnValidationException exception:
                errorKey.Add(exception.ErrorMessages);
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Result = new BadRequestObjectResult(responseError);
                break;
            case NotFoundException notFoundException:
                errorKey.Add(notFoundException.Message);
                context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Result = new NotFoundObjectResult(responseError);
                break;
        }
    }

    private static void ThrowUnknowException(ExceptionContext context)
    {
        var responseError = new ResponseError().Errors;
#if DEBUG
        responseError.Add(context.Exception.Message);
        context.Result = new ObjectResult(responseError);
#else
        responseError.Add(ResourceMessagesException.UNKNOWN_ERROR);
        context.Result = new ObjectResult(new ResponseError());
#endif
    }
}
