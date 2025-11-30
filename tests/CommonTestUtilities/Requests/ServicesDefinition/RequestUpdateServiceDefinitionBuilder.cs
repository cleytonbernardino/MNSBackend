using Bogus;
using MMS.Communication.Requests.ServiceDefinition;
using MMS.Domain.Enums;

namespace CommonTestUtilities.Requests.ServicesDefinition;

public static class RequestUpdateServiceDefinitionBuilder
{
    public static RequestUpdateServiceDefinition Build(string id = "yyy")
    {
        return new Faker<RequestUpdateServiceDefinition>()
            .RuleFor(req => req.Id, id)
            .RuleFor(req => req.Title, f => f.Commerce.ProductName())
            .RuleFor(req => req.Description, f => f.Commerce.ProductDescription())
            .RuleFor(req => req.ServiceType, f => f.Commerce.ProductAdjective())
            .RuleFor(req => req.Status, (ushort)0);
    }
}
