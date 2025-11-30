using MMS.Communication.Responses.ServiceDefinition;

namespace MMS.Application.UseCases.ServiceDefinition.Get;

public interface IGetServiceDefinitionUseCase
{
    Task<ResponseGetServiceDefinition> Execute(long id);
}
