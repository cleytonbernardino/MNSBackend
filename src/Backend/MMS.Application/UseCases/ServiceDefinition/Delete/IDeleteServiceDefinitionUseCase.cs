namespace MMS.Application.UseCases.ServiceDefinition.Delete;

public interface IDeleteServiceDefinitionUseCase
{
    Task Execute(long id);
}
