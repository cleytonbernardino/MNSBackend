namespace MMS.Application.UseCases.Company.Delete;

public interface IDeleteCompanyUseCase
{
    Task Execute(long id);
}
