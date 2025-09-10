using MMS.Communication;

namespace MMS.Application.UseCases.Company.Register;
public interface IRegisterCompanyUseCase
{
    Task Execute(RequestRegisterCompany request);
}
