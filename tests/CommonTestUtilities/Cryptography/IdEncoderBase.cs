using Sqids;

namespace CommonTestUtilities.Cryptography;

public class IdEncoderBase
{
    protected readonly SqidsEncoder<long> Sqids = new(new SqidsOptions
    {
        MinLength = 3,
        Alphabet = "aJVu3P4s0foxAqivmTdrGO1ynS6eMtRLwEFzZkDgCNcj2IHpK7l5bXYWhUBQ89"
    });
}
