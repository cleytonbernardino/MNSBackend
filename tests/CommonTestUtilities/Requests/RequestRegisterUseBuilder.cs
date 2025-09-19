using Bogus;
using MMS.Communication.Requests.User;
using MMS.Domain.Enums;

namespace CommonTestUtilities.Requests;

public static class RequestRegisterUseBuilder
{
    public static RequestRegisterUser Build()
    {
        return new Faker<RequestRegisterUser>()
            .RuleFor(user => user.Email, f => f.Internet.Email())
            .RuleFor(user => user.Phone, () => "11912345678")
            .RuleFor(user => user.FirstName, f => f.Name.FirstName())
            .RuleFor(user => user.LastName, f => f.Name.LastName())
            .RuleFor(user => user.Password, f => f.Internet.Password())
            .RuleFor(user => user.Role, () => (short)UserRolesEnum.MANAGER);
    }
}
