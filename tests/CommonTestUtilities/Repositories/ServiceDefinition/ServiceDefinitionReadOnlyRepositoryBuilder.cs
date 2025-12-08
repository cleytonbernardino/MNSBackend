using MMS.Domain.Repositories.ServiceDefinition;
using Moq;
using Entity = MMS.Domain.Entities;

namespace CommonTestUtilities.Repositories.ServiceDefinition;

public class ServiceDefinitionReadOnlyRepositoryBuilder
{
    private readonly Mock<IServiceDefinitionReadOnlyRepository> _mock = new();

    public IServiceDefinitionReadOnlyRepository Build() => _mock.Object;

    public ServiceDefinitionReadOnlyRepositoryBuilder GetById(Entity.User user, Entity.ServiceDefinition service)
    {
        _mock.Setup(rep => rep.GetById(user, service.Id)).ReturnsAsync(service);
        return this;
    }

    public ServiceDefinitionReadOnlyRepositoryBuilder List(Entity.User user, Entity.ShortServiceDefinition[] services)
    {
        _mock.Setup(rep => rep.List(user)).ReturnsAsync(services);
        return this;
    }
}
