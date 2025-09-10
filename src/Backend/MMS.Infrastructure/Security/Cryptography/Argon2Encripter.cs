using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;
using MMS.Domain.Security.Cryptography;

namespace MMS.Infrastructure.Security.Cryptography;

public class Argon2Encripter : IPasswordEncrypter
{
    private const int SALT_SIZE = 16; // 128 bits
    private const int HASH_SIZE = 32;
    private const int PARALLELISM = 2; // Numero de threads
    private const int INTERATIONS = 3;
    private const int MEMORYKB = 64 * 1024; // Memoria a ser usada para a criptografia em KB


    private static byte[] GenerateSalt()
    {
        byte[] salt = new byte[SALT_SIZE];
        RandomNumberGenerator.Fill(salt);
        return salt;
    }

    private static byte[] HashPassword(string password, byte[] salt)
    {
        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            MemorySize = MEMORYKB,
            DegreeOfParallelism = PARALLELISM,
            Iterations = INTERATIONS
        }; 
        return argon2.GetBytes(HASH_SIZE);
    }

    public string Encrypt(string password)
    {
        byte[] salt = GenerateSalt();

        byte[] hash = HashPassword(password, salt);

        var combinedBytes = new byte[salt.Length + hash.Length];
        Array.Copy(salt, 0, combinedBytes, 0, salt.Length);
        Array.Copy(hash, 0, combinedBytes, salt.Length, hash.Length);

        return Convert.ToBase64String(combinedBytes);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        byte[] combinedBytes = Convert.FromBase64String(hashedPassword);
         
        // Extraindo o salt e hash
        byte[] salt = new byte[SALT_SIZE];
        byte[] hash = new byte[HASH_SIZE];
        Array.Copy(combinedBytes, 0, salt, 0, SALT_SIZE);
        Array.Copy(combinedBytes, SALT_SIZE, hash, 0, HASH_SIZE);

        // fazendo hash da senha digitada
        byte[] newHash = HashPassword(password, salt);

        // Comparando os dois hash
        return CryptographicOperations.FixedTimeEquals(hash, newHash);
    }
}
