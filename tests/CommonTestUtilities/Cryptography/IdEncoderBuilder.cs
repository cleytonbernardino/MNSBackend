using MMS.Application.Services.Encoders;
using Moq;

namespace CommonTestUtilities.Cryptography;

public class IdEncoderBuilder : IdEncoderBase
{
    private readonly Mock<IIdEncoder> _mock = new();

    public IIdEncoder Build() => _mock.Object;

    public IdEncoderBuilder Encoder()
    {
        _mock.Setup(encoder => encoder.Encode(It.IsAny<long>())).Returns<long>(id =>Sqids.Encode(id));
        return this;
    }

    public IdEncoderBuilder Decode()
    {
        _mock.Setup(encoder => encoder.Decode(It.IsAny<string>())).Returns((string id) => Sqids.Decode(id).Single());
        return this;
    }
}
