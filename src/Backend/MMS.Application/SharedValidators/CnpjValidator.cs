using FluentValidation;
using FluentValidation.Validators;
using MMS.Exceptions;

namespace MMS.Application.SharedValidators;

internal class CnpjValidator<T>: PropertyValidator<T, string>
{
    public override string Name => this.GetType().Name;

    public override bool IsValid(ValidationContext<T> context, string cnpj)
    {
        if (string.IsNullOrEmpty(cnpj))
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.CNPJ_EMPTY);
            return false;
        }

        cnpj = RemoveMask(cnpj);
        if (cnpj.Length != 14)
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.CNPJ_INVALID);
            return false;
        }

        string checkDigits = cnpj.Substring(12, 2);
        cnpj = cnpj.Substring(0, 12);
            
        int[] digits = cnpj.Select(c =>
        {
            if (char.IsDigit(c)) return c - '0';
            if (c >= 'A' && c <= 'Z') return (int)c - 48;
            context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.CNPJ_INVALID);
            return -1;
        }).ToArray();

        int fistDigit = CalcDigit(digits);

        int[] digitsWithFirstDigit = digits.Concat([fistDigit]).ToArray();
        int secondDigit = CalcDigit(digitsWithFirstDigit);
        
        return checkDigits == $"{fistDigit}{secondDigit}";
    }

    protected override string GetDefaultMessageTemplate(string errorCode) => "{ErrorMessage}";

    private static int CalcDigit(int[] digits)
    {
        int mult = 2;
        int result = 0;
        
        for (int i = digits.Length - 1; i >= 0; i--)
        {
            result += digits[i] * mult;
            mult++;
            if (mult > 9)
                mult = 2;
        }

        result = result % 11;
        if (result < 2)
            return 0;
        return 11 - result;
    }

    private static string RemoveMask(string cnpj) => new string(cnpj.Where(char.IsLetterOrDigit).ToArray()).ToUpper();
}
