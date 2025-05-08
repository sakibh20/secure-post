using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class SecureDataHandler
{
    private static readonly string filePath = Path.Combine(Application.persistentDataPath, "user.dat");

    private static readonly byte[] key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes("YourStrongKeyHere123!"));
    private static readonly byte[] iv = Encoding.UTF8.GetBytes("1234567890123456");

    public static void SaveCredentials(string userId, string password)
    {
        string combined = $"{userId}:{password}";
        byte[] encrypted = EncryptStringToBytes(combined);
        File.WriteAllBytes(filePath, encrypted);
    }

    public static (string userId, string password) LoadCredentials()
    {
        if (!File.Exists(filePath)) return (null, null);

        byte[] encrypted = File.ReadAllBytes(filePath);
        string decrypted = DecryptStringFromBytes(encrypted);
        var parts = decrypted.Split(':');
        return (parts[0], parts[1]);
    }

    private static byte[] EncryptStringToBytes(string plainText)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;
            aesAlg.Padding = PaddingMode.PKCS7;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor();
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (StreamWriter sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
                sw.Close();
                return ms.ToArray();
            }
        }
    }

    private static string DecryptStringFromBytes(byte[] cipherText)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;
            aesAlg.Padding = PaddingMode.PKCS7;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor();
            using (MemoryStream ms = new MemoryStream(cipherText))
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (StreamReader sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
