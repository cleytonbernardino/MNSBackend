using MMS.Communication;

namespace MMS.Application.UseCases.Login.DoLogin;

public interface IDoLoginUseCase
{
    Task<ResponseRegisteredUser> Execute(RequestLogin request);
}
