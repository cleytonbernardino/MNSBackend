using MMS.Communication.Requests.Auth;
using MMS.Communication.Responses.Auth;

namespace MMS.Application.UseCases.Auth.DoLogin;

public interface IDoLoginUseCase
{
    Task<ResponseDoLogin> Execute(RequestDoLogin request);
    string? GetRefreshToken();
}
