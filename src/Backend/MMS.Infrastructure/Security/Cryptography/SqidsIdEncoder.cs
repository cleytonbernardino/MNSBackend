using MMS.Application.Services.Encoders;
using Sqids;

namespace MMS.Infrastructure.Security.Cryptography;

public class SqidsIdEncoder(
    SqidsEncoder<long> idEncoder
    ) : IIdEncoder
{
    private readonly SqidsEncoder<long> _idEncoder = idEncoder;

    public long Decode(string encryptedId) => _idEncoder.Decode(encryptedId).Single();

    public string Encode(long id) => _idEncoder.Encode(id);
}
