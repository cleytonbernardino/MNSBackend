namespace CommonTestUtilities.Cryptography;

public class IdEncoderForTests : IdEncoderBase
{
    public string Encode(long id) => Sqids.Encode(id);
    public long Decode(string id) => Sqids.Decode(id).Single();
}
