using MMS.Communication.Requests.Auth;
using MMS.Communication.Responses.Auth;

namespace MMS.Application.UseCases.Auth.RefreshToken;

public interface IRefreshTokenUseCase
{
    Task<ResponseRefreshToken> Execute(RequestRefreshToken request, string refreshToken);
    string? GetRefreshToken();
}
