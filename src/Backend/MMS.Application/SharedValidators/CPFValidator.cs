using FluentValidation;
using FluentValidation.Validators;

namespace MMS.Application.SharedValidators;

internal class CPFValidator<T>: PropertyValidator<T, string>
{
    public override string Name => this.GetType().Name;

    public override bool IsValid(ValidationContext<T> context, string cpf)
    {
        if (string.IsNullOrEmpty(cpf))
            return false;

        cpf = RemoveMask(cpf);
        if (cpf.Length != 11)
            return false;
        
        int firstDigit  = CalcDigit(cpf.Substring(0, 9));
        int secondDigit = CalcDigit(cpf.Substring(0, 10));
        
        return $"{firstDigit}{secondDigit}" == cpf.Substring(10, 2);
    }

    protected override string GetDefaultMessageTemplate(string errorCode) => "{ErrorMessage}";

    private static int CalcDigit(string cpf)
    {
        int mult = cpf.Length + 1;
        int result = 0;

        for (int i = 0; i <=cpf.Length; i++)
        {
            bool isValid = int.TryParse(cpf[i].ToString(), out int value);
            if (!isValid)
                return -1;

            result += value * mult;
            mult--;
        }

        result = result % 11;
        if (result < 2)
            return 0;

        return (11 - result);
    }

    private static string RemoveMask(string cpf) => new string(cpf.Where(char.IsDigit).ToArray());
}
