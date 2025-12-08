using Bogus;
using MMS.Domain.Entities;

namespace CommonTestUtilities.Entities;

public static class ShortServicesDefinitionBuilder
{
    public static ShortServiceDefinition Build()
    {
        return new Faker<ShortServiceDefinition>()
            .RuleFor(service => service.Title, f => f.Commerce.ProductName())
            .RuleFor(service => service.Description, f => f.Commerce.ProductDescription());
    }

    public static ShortServiceDefinition[] BuildInBatch(ushort count = 2)
    {
        ShortServiceDefinition[] services = new ShortServiceDefinition[count];
        for (ushort i = 0; i < count; i++)
        {
            services[i] = Build();
        }
        return services;
    }
}
