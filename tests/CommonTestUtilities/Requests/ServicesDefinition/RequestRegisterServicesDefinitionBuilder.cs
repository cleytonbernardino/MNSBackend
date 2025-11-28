using Bogus;
using MMS.Communication.Requests.ServiceDefinition;
using MMS.Domain.Enums;

namespace CommonTestUtilities.Requests.ServicesDefinition;

public static class RequestRegisterServiceDefinitionBuilder
{
    public static RequestRegisterServices Build()
    {
        return new Faker<RequestRegisterServices>()
            .RuleFor(req => req.Title, f => f.Commerce.ProductName())
            .RuleFor(req => req.Description, f => f.Commerce.ProductDescription())
            .RuleFor(req => req.ServiceType, f => f.Commerce.ProductAdjective())
            .RuleFor(req => req.Status, (short)ServicesStatusEnum.PENDING);
    }

    public static RequestRegisterServices[] BuildInBatch(uint count = 2)
    {
        List<RequestRegisterServices> requests = [];

        for (uint i = 0; i < count; i++)
        {
            requests.Add(Build());
        }

        return requests.ToArray();
    }
}
