using Bogus;
using CommonTestUtilities.Cryptography;
using MMS.Communication.Requests.User;
using MMS.Domain.Enums;

namespace CommonTestUtilities.Requests;

public static class RequestUpdateUserBuilder
{
    public static RequestUpdateUser Build()
    {
        IdEncoderForTests encoder = new();
        return new Faker<RequestUpdateUser>()
            .RuleFor(req => req.UserIdToUpdate, f => encoder.Encode(f.Random.Int(1, 100)))
            .RuleFor(req => req.LastName, f => f.Person.LastName)
            .RuleFor(req => req.Email, f => f.Person.Email)
            .RuleFor(req => req.Phone, () => "11987424156")
            .RuleFor(req => req.Role, () => (short)UserRolesEnum.MANAGER);
    }
}
