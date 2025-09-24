using FluentValidation;
using MMS.Communication.Requests.Company;
using MMS.Exceptions;

namespace MMS.Application.UseCases.Company.Update;

public class UpdateCompanyValidator: AbstractValidator<RequestUpdateCompany>
{
    public UpdateCompanyValidator()
    {
        RuleFor(req => req.DoingBusinessAs).NotEmpty().WithMessage(ResourceMessagesException.DBA_EMPTY);
        RuleFor(req => req.CEP).NotEmpty().WithMessage(ResourceMessagesException.CEP_EMPTY);
        RuleFor(req => req.AddressNumber).NotEmpty().WithMessage(ResourceMessagesException.ADDRESS_NUMBER_EMPTY);
        RuleFor(req => req.PhoneNumber).NotEmpty().WithMessage(ResourceMessagesException.PHONE_EMPTY);
        When(req => !string.IsNullOrWhiteSpace(req.DoingBusinessAs), () =>
                RuleFor(req => req.DoingBusinessAs).MaximumLength(100)
                    .WithMessage(ResourceMessagesException.DBA_MAX_LENGTH)
        );
        When(req => !string.IsNullOrWhiteSpace(req.BusinessEmail), () =>
            RuleFor(request => request.BusinessEmail).EmailAddress()
                .WithMessage(ResourceMessagesException.INVALID_EMAIL)
        );
        When(req => !string.IsNullOrWhiteSpace(req.PhoneNumber), () =>
            RuleFor(request => request.PhoneNumber).Matches(@"^\(?\d{2}\)?\s?9\d{4}-?\d{4}$")
                .WithMessage(ResourceMessagesException.PHONE_NOT_VALID)
        );
        When(req => !string.IsNullOrWhiteSpace(req.CEP), () =>
            RuleFor(request => request.CEP).Matches(@"^\d{5}-?\d{3}$")
                .WithMessage(ResourceMessagesException.CEP_INVALID)
        );
    }
}
