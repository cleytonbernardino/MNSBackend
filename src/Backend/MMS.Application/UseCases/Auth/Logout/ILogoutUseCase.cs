using MMS.Communication.Requests.Auth;

namespace MMS.Application.UseCases.Auth.Logout;

public interface ILogoutUseCase
{
    Task Execute(string refreshToken, string accessToken);
}
