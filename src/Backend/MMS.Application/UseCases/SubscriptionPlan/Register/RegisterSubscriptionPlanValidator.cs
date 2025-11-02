using FluentValidation;
using MMS.Communication.Requests.SubscriptionsPlans;
using MMS.Exceptions;

namespace MMS.Application.UseCases.SubscriptionPlan.Register;

public class RegisterSubscriptionPlanValidator : AbstractValidator<RequestRegisterSubscriptionPlan>
{
    public RegisterSubscriptionPlanValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage(ResourceMessagesException.NAME_CANNOT_BE_EMPTY);
        RuleFor(request => request.Description).NotEmpty().WithMessage(ResourceMessagesException.DESCRIPTION_CANNOT_BE_EMPTY);
        RuleFor(request => request.Properties).NotEmpty().WithMessage(ResourceMessagesException.EMPTY_PROPERTIES);
    }
}
