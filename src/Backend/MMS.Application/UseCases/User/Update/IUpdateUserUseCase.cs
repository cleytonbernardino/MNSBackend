using MMS.Communication.Requests.User;

namespace MMS.Application.UseCases.User.Update;

public interface IUpdateUserUseCase
{
    Task Execute(RequestUpdateUser request);
}
