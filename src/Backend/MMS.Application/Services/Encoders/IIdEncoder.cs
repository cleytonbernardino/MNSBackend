namespace MMS.Application.Services.Encoders;

public interface IIdEncoder
{
    string Encode(long id);
    long Decode(string encryptedId);
}
