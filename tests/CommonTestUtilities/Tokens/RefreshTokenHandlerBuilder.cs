using Moq;
using MMS.Domain.Entities;
using MMS.Domain.Enums;
using MMS.Domain.Security.Token;

namespace CommonTestUtilities.Tokens;

public class RefreshTokenHandlerBuilder
{
    private readonly Mock<IRefreshTokenHandler> _mock = new();

    public IRefreshTokenHandler Build() => _mock.Object;

    public RefreshTokenHandlerBuilder GenerateTokenAndSave(string tokenToMock = "TestingToken")
    {
        _mock.Setup(mock => mock.GenerateTokenAndSave(It.IsAny<Guid>())).ReturnsAsync(tokenToMock);
        return this;
    }
    
    public RefreshTokenHandlerBuilder GenerateTokenAndSave(Guid userIdentifier, string tokenToMock = "TestingToken")
    {
        _mock.Setup(mock => mock.GenerateTokenAndSave(userIdentifier)).ReturnsAsync(tokenToMock);
        return this;
    }
    
    public RefreshTokenHandlerBuilder GetToken(RefreshToken token)
    {
        _mock.Setup(mock => mock.GetToken(
            It.IsAny<string>(),
            It.IsAny<Guid>()
            )).ReturnsAsync(token);
        return this;
    }

    public RefreshTokenHandlerBuilder GetDataFromAccessToken(Guid userIdentifier)
    {
        _mock.Setup(mock => mock.GetDataFromAccessToken(
            It.IsAny<string>()
            )).Returns((userIdentifier, UserRolesEnum.MANAGER.ToString(), false));
        return this;
    }
}
