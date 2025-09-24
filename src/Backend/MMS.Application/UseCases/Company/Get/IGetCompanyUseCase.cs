using MMS.Communication.Responses.Company;

namespace MMS.Application.UseCases.Company.Get;

public interface IGetCompanyUseCase
{
    Task<ResponseCompany> Execute(long id);
}
