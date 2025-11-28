using FluentValidation;
using MMS.Communication.Requests.ServiceDefinition;
using MMS.Exceptions;

namespace MMS.Application.UseCases.ServiceDefinition.Register;

public class RegisterServicesValidator: AbstractValidator<RequestRegisterServices>
{
    public RegisterServicesValidator()
    {
        RuleFor(request => request.Title).NotEmpty().WithMessage(ResourceMessagesException.EMPTY_TITLE);
        RuleFor(request => request.ServiceType).NotEmpty().WithMessage(ResourceMessagesException.EMPTY_SERVICE_TYPE);
    }
}
