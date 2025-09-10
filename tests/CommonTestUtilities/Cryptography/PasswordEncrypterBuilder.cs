using MMS.Domain.Security.Cryptography;
using MMS.Infrastructure.Security.Cryptography;

namespace CommonTestUtilities.Cryptography;

public static class PasswordEncrypterBuilder
{
    public static IPasswordEncrypter Build() => new Argon2Encripter();
}
