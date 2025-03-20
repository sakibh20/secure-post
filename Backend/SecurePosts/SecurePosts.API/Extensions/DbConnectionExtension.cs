using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecurePosts.Context;

namespace SecurePosts.API.Extensions
{
    public static class DbConnectionExtension
    {
        static private string g_fixed_key = "@wf8y6t_*4zkjd78";
        //Used for AesEncryption and AesDecryption
        static private byte[] iv16Bit = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        public static void ConfigureOracleContext(this IServiceCollection services, IConfiguration config)
        {
            string DB_CON = string.Empty;
            var IS_ENCRYPTED_DB_CON = config.GetConnectionString("IS_ENCRYPTED");
            if(IS_ENCRYPTED_DB_CON == "1")
                DB_CON = GetPlainText(config.GetConnectionString("DB_CON"));
            else
                DB_CON = config.GetConnectionString("DB_CON");

            services.AddDbContext<SecurePostsDbContext>(options =>
                        options.UseMySql(DB_CON,
                            new MySqlServerVersion(new Version(8, 0, 31)) // Default MySQL version
                        ));
        }

        public static string AesDecrypt(string dataToDecrypt, byte[] keyX)
        {

            var bytes = Convert.FromBase64String(dataToDecrypt);
            using (var aes = new AesCryptoServiceProvider())
            {
                using (var ms = new MemoryStream())
                using (var decryptor = aes.CreateDecryptor(keyX, iv16Bit))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                {
                    cs.Write(bytes, 0, bytes.Length);
                    cs.FlushFinalBlock();
                    var cipher = ms.ToArray();
                    // return Encoding.UTF8.GetString(cipher);
                    string sad = Encoding.UTF8.GetString(cipher);
                    return sad;
                }
            }
        }

        public static string GetPlainText(string encryptedPassword)
        {
            if (!string.IsNullOrEmpty(encryptedPassword))
            {
                string eKey = encryptedPassword.Substring(0, 44);
                string eText = encryptedPassword.Substring(44, encryptedPassword.Length - 44);

                Byte[] fixedKey = new Byte[32];
                fixedKey = Encoding.Unicode.GetBytes(g_fixed_key);

                string plainPassword = AesDecrypt(eKey, fixedKey);
                byte[] decryptedRandomkey = Encoding.Unicode.GetBytes(plainPassword);

                string plainText = AesDecrypt(eText, decryptedRandomkey);
                return plainText;
            }
            else
            {
                return string.Empty;
            }

        }

    }
}
