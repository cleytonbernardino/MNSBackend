using Bogus;
using MMS.Domain.Entities;
using MMS.Domain.Enums;

namespace CommonTestUtilities.Entities;

public static class ServiceDefinitionBuilder
{
    public static ServiceDefinition Build(long? companyId = null, Guid? registeredBy = null)
    {
        return new Faker<ServiceDefinition>()
            .RuleFor(service => service.Id, 0)
            .RuleFor(service => service.Title, f => f.Commerce.ProductName())
            .RuleFor(service => service.Description, f => f.Commerce.ProductDescription())
            .RuleFor(service => service.Status, ServicesStatusEnum.PENDING)
            .RuleFor(service => service.CompanyId, companyId ?? 0)
            .RuleFor(service => service.RegisteredBy, registeredBy ?? Guid.Empty)
            .RuleFor(service => service.ServiceType, f => f.Commerce.ProductAdjective());
    }

    public static ServiceDefinition[] BuildInBatch(uint count = 2)
    {
        ServiceDefinition[] services = new ServiceDefinition[count];

        for (int i = 0; i < count; i++)
        {
            services[i] = Build();
        }

        return services;
    }
}
