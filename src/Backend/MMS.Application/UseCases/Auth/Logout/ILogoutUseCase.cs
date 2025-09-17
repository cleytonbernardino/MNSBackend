using MMS.Communication;

namespace MMS.Application.UseCases.Auth.Logout;

public interface ILogoutUseCase
{
    Task Execute(RequestRefreshAccessToken request, string refreshToken);
}
