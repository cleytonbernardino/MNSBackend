using Entity = MMS.Domain.Entities;

namespace MMS.Domain.Repositories.ServiceDefinition;

public interface IServiceDefinitionReadOnlyRepository
{
    Task<Entity.ServiceDefinition?> GetById(Entity.User user, long id);
    Task<Entity.ShortServiceDefinition[]> List(Entity.User user);
}
