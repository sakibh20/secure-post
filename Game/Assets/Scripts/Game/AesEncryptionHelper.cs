using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class AesEncryptionHelper
{
    private readonly byte[] _key;
    private readonly string _baseKey = "40YPHs5wzB/Q5Blg7EN5h75pHzut9TkvUYUOItfMdP4=+EQ/v7/URS2b8dgIpx6ekQ==";
    
    public AesEncryptionHelper()
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            _key = sha256.ComputeHash(Encoding.UTF8.GetBytes(_baseKey)); // 256-bit AES key
        }
    }
    
    // public static string GenerateKey(int byteLength = 32)
    // {
    //     byte[] keyBytes = new byte[byteLength];
    //     using (var rng = new RNGCryptoServiceProvider())
    //     {
    //         rng.GetBytes(keyBytes);
    //     }
    //     return Convert.ToBase64String(keyBytes);
    // }

    public string Encrypt(string plainText)
    {
        byte[] iv = new byte[16]; // 128-bit IV
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(iv); // Random IV per message
        }

        using (Aes aes = Aes.Create())
        {
            aes.Key = _key;
            aes.IV = iv;
            aes.Padding = PaddingMode.PKCS7;

            using (var ms = new MemoryStream())
            {
                // Prepend IV
                ms.Write(iv, 0, iv.Length);

                using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }

                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }
}
