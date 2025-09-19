using MMS.Communication.Requests.Auth;

namespace MMS.Application.UseCases.Auth.Logout;

public interface ILogoutUseCase
{
    Task Execute(RequestRefreshToken request, string refreshToken);
}
