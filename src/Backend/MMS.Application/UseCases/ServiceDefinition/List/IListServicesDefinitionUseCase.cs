using MMS.Communication.Responses.ServiceDefinition;

namespace MMS.Application.UseCases.ServiceDefinition.List;

public interface IListServicesDefinitionUseCase
{
    Task<ResponseShortServicesDefinition> Execute();
}
