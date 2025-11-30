using Entity = MMS.Domain.Entities;

namespace MMS.Domain.Repositories.ServiceDefinition;

public interface IServiceDefinitionUpdateOnlyRepository
{
    Task<Entity.ServiceDefinition?> GetById(Entity.User user, long id);
    void Update(Entity.ServiceDefinition service);
    void Delete(Entity.ServiceDefinition serviceDefinition);
}
