using MMS.Communication.Responses.Company;

namespace MMS.Application.UseCases.Company.List;

public interface IListCompaniesUseCase
{
    Task<ResponseShortCompanies> Execute();
}
