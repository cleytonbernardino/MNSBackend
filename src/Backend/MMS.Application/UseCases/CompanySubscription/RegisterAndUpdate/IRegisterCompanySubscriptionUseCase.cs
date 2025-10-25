using MMS.Communication.Requests.Company;

namespace MMS.Application.UseCases.CompanySubscription.RegisterAndUpdate;

public interface IRegisterCompanySubscriptionUseCase
{
    Task Execute(RequestRegisterCompanySubscription request);
}
