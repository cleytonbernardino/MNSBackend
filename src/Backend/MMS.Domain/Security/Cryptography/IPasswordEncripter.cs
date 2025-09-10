namespace MMS.Domain.Security.Cryptography;

public interface IPasswordEncrypter
{
    string Encrypt(string password);
    bool VerifyPassword(string password, string hashedPassword);
}
