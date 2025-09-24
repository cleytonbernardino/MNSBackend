using MMS.Communication.Requests.Company;

namespace MMS.Application.UseCases.Company.Update;

public interface IUpdateCompanyUseCase
{
    Task Execute(RequestUpdateCompany request, long id);
}
