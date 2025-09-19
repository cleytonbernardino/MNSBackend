using FluentValidation;
using MMS.Application.SharedValidators;
using MMS.Communication.Requests.Auth;
using MMS.Exceptions;

namespace MMS.Application.UseCases.Auth.DoLogin;

public class DoLoginValidator : AbstractValidator<RequestDoLogin>
{
    public DoLoginValidator()
    {
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);
        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestDoLogin>());
        When(user =>
                !string.IsNullOrEmpty(user.Email),
            () => RuleFor(u => u.Email).EmailAddress().WithMessage(ResourceMessagesException.INVALID_EMAIL)
        );
    }
}
