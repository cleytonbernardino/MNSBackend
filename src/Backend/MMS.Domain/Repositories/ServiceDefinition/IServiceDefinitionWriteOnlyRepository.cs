using MMS.Domain.Entities;

namespace MMS.Domain.Repositories.ServiceDefinition;

public interface IServiceDefinitionWriteOnlyRepository
{
    Task Register(Entities.ServiceDefinition serviceDefinition);
}
