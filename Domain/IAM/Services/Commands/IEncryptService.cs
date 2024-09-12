namespace _2_Domain.IAM.Services.Commands;

public interface IEncryptService
{
    string Encrypt(string value);
    
    bool Verify(string password, string passwordHashed);
}