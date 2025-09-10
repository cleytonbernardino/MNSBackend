using MMS.Communication;

namespace MMS.Application.UseCases.Login.RefreshToken;

public interface IRefreshTokenUseCase
{
    Task<ResponseToken> Execute(RequestRefreshToken request);
}
