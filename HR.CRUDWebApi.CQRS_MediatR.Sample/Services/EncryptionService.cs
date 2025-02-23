using HR.CRUDWebApi.CQRS_MediatR.Sample.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace HR.CRUDWebApi.CQRS_MediatR.Sample.Services
{
    public class EncryptionService : IEncryptionService
    {
        public readonly byte[] _key;
        public readonly byte[] _iv;
        public readonly IConfiguration _configuration;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public EncryptionService(IConfiguration configuration)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
            _configuration = configuration;
            var key = _configuration["EncryptionKey"];
            if (key != null)
            {
                _key = Encoding.UTF8.GetBytes(key.Substring(0, 32));
                _iv = Encoding.UTF8.GetBytes(key.Substring(32, 16));
            }
        }
        public string Decrypt(string cipherText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using var ms = new MemoryStream(cipherBytes);
                using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                using var sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
        }

        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                using var ms = new MemoryStream();
                using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
                cs.Write(plainBytes, 0, plainBytes.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }
}
