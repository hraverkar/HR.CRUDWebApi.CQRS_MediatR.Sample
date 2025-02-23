namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Interfaces
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }
}
