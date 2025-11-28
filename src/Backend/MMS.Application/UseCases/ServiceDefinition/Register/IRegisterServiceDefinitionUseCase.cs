using MMS.Communication.Requests.ServiceDefinition;

namespace MMS.Application.UseCases.ServiceDefinition.Register;

public interface IRegisterServicesUseCase
{
    Task Execute(RequestRegisterServices request);
}
