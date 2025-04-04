using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using SecureService.DAL.Repositories;
using SecureService.Logging;

namespace SecureService.DAL.Services
{
    public class DalClsService : IDalClsRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogRepository _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly byte[] _key;
        private readonly byte[] _iv;
        private readonly string key = "40YPHs5wzB/Q5Blg7EN5h75pHzut9TkvUYUOItfMdP4=+EQ/v7/URS2b8dgIpx6ekQ==";

        public DalClsService(ILogRepository logger,
            IConfiguration config,
            IMemoryCache memoryCache)
        {
            this._logger = logger;
            this._config = config;
            this._memoryCache = memoryCache;

            using (SHA256 sha256 = SHA256.Create())
            {
                _key = sha256.ComputeHash(Encoding.UTF8.GetBytes(key)); // Hash key to 256-bit
            }

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                _iv = new byte[16]; // AES requires 16-byte IV
                rng.GetBytes(_iv); // Generate random IV (secure)
            }
        }


        public string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _key;
                aesAlg.IV = _iv;
                aesAlg.Padding = PaddingMode.PKCS7;

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }

                        byte[] encryptedBytes = msEncrypt.ToArray();
                        byte[] result = new byte[_iv.Length + encryptedBytes.Length];
                        Buffer.BlockCopy(_iv, 0, result, 0, _iv.Length);
                        Buffer.BlockCopy(encryptedBytes, 0, result, _iv.Length, encryptedBytes.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }
        public string Decrypt(string cipherText)
        {
            byte[] fullCipher = Convert.FromBase64String(cipherText);

            byte[] iv = new byte[16]; // Extract IV from encrypted text
            byte[] cipherBytes = new byte[fullCipher.Length - iv.Length];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipherBytes, 0, cipherBytes.Length);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _key;
                aesAlg.IV = iv;
                aesAlg.Padding = PaddingMode.PKCS7;

                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }
        public string EncryptSha256Hash(string plainText)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(plainText);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToHexString(hashBytes);
            }
        }
        public DateTime ConvertToDateTime(string pDateValue)
        {
            try
            {
                if (string.IsNullOrEmpty(pDateValue))
                    return new DateTime();

                string allowedformates = "dd/MM/yyyy,dd/MM/yyyy HH:mm:ss,yyyy-MM-dd HH:mm:ss,dd-MMM-yy,dd-MMM-yyyy,dd-MM-yyyy HH:mm:ss,dd/MM/yyyy HH.mm.ss,dd/MM/yyyy H.mm.ss,dd MMM yyyy HH.mm.ss,dd MMM yyyy H.mm.ss,d MMM yyyy H.mm.ss";

                if (!string.IsNullOrEmpty(allowedformates))
                {
                    var ci = new CultureInfo("en-US");
                    string[] stringSeparators = new string[] { "," };
                    string[] allowedformatesarray = allowedformates.Split(stringSeparators, StringSplitOptions.None);
                    string[] finalformats = allowedformatesarray.Union(ci.DateTimeFormat.GetAllDateTimePatterns()).ToArray();
                    return DateTime.ParseExact(pDateValue, finalformats, ci, DateTimeStyles.AssumeLocal);
                }
                else
                {
                    throw new Exception(pDateValue + " -> this DateTime format is not allowed.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}