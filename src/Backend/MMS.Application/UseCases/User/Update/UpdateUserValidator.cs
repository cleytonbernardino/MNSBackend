using FluentValidation;
using MMS.Communication.Requests.User;
using MMS.Domain.Enums;
using MMS.Exceptions;

namespace MMS.Application.UseCases.User.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUser>
{
    public UpdateUserValidator()
    {
        RuleFor(user => user.Role).Must(role => Enum.IsDefined(typeof(UserRolesEnum), role))
            .WithMessage(ResourceMessagesException.ROLE_INVALID);
        When(user => !string.IsNullOrEmpty(user.Email), () =>
        {
            RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesException.INVALID_EMAIL);
        });
        When(user => !string.IsNullOrEmpty(user.Phone), () =>
            RuleFor(user => user.Phone).Matches(@"^\(?\d{2}\)?\s?9\d{4}-?\d{4}$")
                .WithMessage(ResourceMessagesException.PHONE_NOT_VALID)
        );
    }
}
