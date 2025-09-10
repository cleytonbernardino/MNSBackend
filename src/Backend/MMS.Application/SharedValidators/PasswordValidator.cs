using FluentValidation;
using FluentValidation.Validators;
using MMS.Exceptions;

namespace MMS.Application.SharedValidators;

internal class PasswordValidator<T> : PropertyValidator<T, string>
{
    public override string Name => "PasswordValidator";

    public override bool IsValid(ValidationContext<T> context, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.PASSWORD_EMPTY);
            return false;
        }else if (password.Length < 6)
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.PASSWORD_LENGTH_IS_INVALID);
            return false;
        }

        return true;
    }

    protected override string GetDefaultMessageTemplate(string errorCode) => "{ErrorMessage}";
}
