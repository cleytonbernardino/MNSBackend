using MMS.Communication.Requests.ServiceDefinition;

namespace MMS.Application.UseCases.ServiceDefinition.Update;

public interface IUpdateServicesDefinitionUseCase
{
    Task Execute(RequestUpdateServiceDefinition request);
}
