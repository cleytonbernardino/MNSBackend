using MMS.Communication.Requests.User;

namespace MMS.Application.UseCases.User.Register;

public interface IRegisterUserUseCase
{
    Task Execute(RequestRegisterUser request);
}
