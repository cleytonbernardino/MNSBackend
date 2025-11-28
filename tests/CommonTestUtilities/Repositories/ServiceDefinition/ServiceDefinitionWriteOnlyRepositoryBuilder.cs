using MMS.Domain.Repositories;
using MMS.Domain.Repositories.ServiceDefinition;
using Moq;

namespace CommonTestUtilities.Repositories.ServiceDefinition;

public static class ServiceDefinitionWriteOnlyRepositoryBuilder
{
    public static IServiceDefinitionWriteOnlyRepository Build()
    {
        return new Mock<IServiceDefinitionWriteOnlyRepository>().Object;
    }
}
