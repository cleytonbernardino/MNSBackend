using FluentValidation;
using MMS.Communication.Requests.Company;
using MMS.Exceptions;

namespace MMS.Application.UseCases.CompanySubscription.RegisterAndUpdate;

public class RegisterCompanySubscriptionValidator : AbstractValidator<RequestRegisterCompanySubscription>
{
    public RegisterCompanySubscriptionValidator()
    {
        RuleFor(request => request.CompanyId).NotEmpty().WithMessage(ResourceMessagesException.EMPTY_COMPANY_ID);
        RuleFor(request => request.SubscriptionId).NotEmpty().WithMessage(ResourceMessagesException.EMPTY_SUBSCRIPTION_ID);
    }
}
