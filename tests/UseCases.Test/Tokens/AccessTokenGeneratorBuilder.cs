using Moq;
using MMS.Domain.Enums;
using MMS.Domain.Security.Token;
using Entity = MMS.Domain.Entities;

namespace UseCases.Test.Tokens;

internal static class AccessTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build(string tokenToMock = "AccessToken")
    {
        var mock = new Mock<IAccessTokenGenerator>();
        mock.Setup(m => m.Generate(
            It.IsAny<Guid>(), It.IsAny<UserRolesEnum>())
        ).Returns(tokenToMock);
        
        return mock.Object;
    }
    
    public static IAccessTokenGenerator Build(Entity.User user, string tokenToMock = "AccessToken")
    {
        var mock = new Mock<IAccessTokenGenerator>();
        mock.Setup(m => m.Generate(user.UserIdentifier, user.Role)).Returns(tokenToMock);
        
        return mock.Object;
    }
    
}
