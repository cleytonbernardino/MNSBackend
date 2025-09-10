using Shouldly;
using MMS.Infrastructure.Security.Cryptography;

namespace Validators.Test.Cryptography;

public class EncrypterTest
{
    [Fact]
    public void Success()
    {
        var encrypter = new Argon2Encripter();

        const string password = "TestPassword";

        var encrypterPassword = encrypter.Encrypt(password);

        encrypterPassword.ShouldNotBe(password);
    }

    [Fact]
    public void Success_Verify_Password()
    {
        var encrypter = new Argon2Encripter();

        const string password = "TestPassword";

        var encrypterPassword = encrypter.Encrypt(password);

        encrypter.VerifyPassword(password, encrypterPassword).ShouldBeTrue();
    }

    [Fact]
    public void Error_Incorrect_Password()
    {
        var encrypter = new Argon2Encripter();

        const string password = "TestPassword";
        const string incorrectPassword = "TestPassword123";

        var encrypterPassword = encrypter.Encrypt(password);

        encrypter.VerifyPassword(incorrectPassword, encrypterPassword).ShouldBeFalse();
    }
}
