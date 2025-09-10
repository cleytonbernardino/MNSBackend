using MMS.Communication;

namespace MMS.Application.UseCases.User.Register;

public interface IRegisterUserUseCase
{
    Task<ResponseRegisteredUser> Execute(RequestRegisterUser request);
}
