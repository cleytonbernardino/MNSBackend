using MMS.Communication;

namespace MMS.Application.UseCases.User.Update.Password;

public interface IUpdateUserPasswordUseCase
{
    Task Execute(RequestUpdateUserPassword request);
}
