using MMS.Communication;

namespace MMS.Application.UseCases.User.Update;

public interface IUpdateUserUseCase
{
    Task Execute(RequestUpdateUser request);
}
