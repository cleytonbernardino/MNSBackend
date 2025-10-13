using MMS.Domain.Services.Cache;
using Moq;

namespace CommonTestUtilities.Cache;

public class CacheServiceBuilder
{
    private readonly Mock<ICacheService> _mock = new();

    public ICacheService Build()
    {
        return _mock.Object;
    }

}
