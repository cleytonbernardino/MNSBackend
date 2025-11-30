using MMS.Domain.Repositories.ServiceDefinition;
using Moq;
using Entity = MMS.Domain.Entities;

namespace CommonTestUtilities.Repositories.ServiceDefinition;

public class ServiceDefinitionUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IServiceDefinitionUpdateOnlyRepository> _mock = new();

    public IServiceDefinitionUpdateOnlyRepository Build() => _mock.Object;

    public ServiceDefinitionUpdateOnlyRepositoryBuilder GetById(Entity.User user, Entity.ServiceDefinition service)
    {
        _mock.Setup(rep => rep.GetById(user, service.Id)).ReturnsAsync(service);
        return this;
    }
}
