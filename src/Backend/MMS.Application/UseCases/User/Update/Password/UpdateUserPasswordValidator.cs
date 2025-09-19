using FluentValidation;
using MMS.Application.SharedValidators;
using MMS.Communication.Requests.User;

namespace MMS.Application.UseCases.User.Update.Password;

public class UpdateUserPasswordValidator : AbstractValidator<RequestUpdateUserPassword>
{
    public UpdateUserPasswordValidator()
    {
        RuleFor(req => req.OldPassword).SetValidator(new PasswordValidator<RequestUpdateUserPassword>());
        RuleFor(req => req.NewPassword).SetValidator(new PasswordValidator<RequestUpdateUserPassword>());
    }
}
