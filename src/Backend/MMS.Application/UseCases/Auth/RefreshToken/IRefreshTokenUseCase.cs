using MMS.Communication;

namespace MMS.Application.UseCases.Auth.RefreshToken;

public interface IRefreshTokenUseCase
{
    Task<ResponseToken> Execute(RequestRefreshAccessToken request, string refreshToken);
}
