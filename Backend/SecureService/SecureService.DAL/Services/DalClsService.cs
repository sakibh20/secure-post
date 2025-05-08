using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SecureService.DAL.Repositories;
using SecureService.Entity.Shared.Status;
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
        private readonly string key;
        private readonly string game_engine_key;

        public DalClsService(ILogRepository logger,
            IConfiguration config,
            IMemoryCache memoryCache)
        {
            this._logger = logger;
            this._config = config;
            this._memoryCache = memoryCache;

            this.key = Environment.GetEnvironmentVariable("AES_ENCRYPTION_KEY");
            this.game_engine_key = Environment.GetEnvironmentVariable("WEBSECRET_KEY");
            if (string.IsNullOrEmpty(this.key))
            {
                throw new Exception("AES Encryption key not found in environment variables.");
            }

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
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;
            try
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
            catch(Exception ex)
            {
                throw new Exception("Decryption Failed.");
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

                string allowedformates = "dd/MM/yyyy HH:mm:ss,dd-MM-yyyy HH:mm:ss,dd.MM.yyyy HH:mm:ss,dd MM yyyy HH:mm:ss,MM/dd/yyyy HH:mm:ss,MM-dd-yyyy HH:mm:ss,MM.dd.yyyy HH:mm:ss,yyyy/MM/dd HH:mm:ss,yyyy-MM-dd HH:mm:ss,yyyy.MM.dd HH:mm:ss,yyyy MM dd HH:mm:ss,d/M/yyyy HH:mm:ss,d-M-yyyy HH:mm:ss,d.M.yyyy HH:mm:ss,M/d/yyyy HH:mm:ss,M-d-yyyy HH:mm:ss,M.d.yyyy HH:mm:ss,dd/MM/yyyy hh:mm:ss tt,dd-MM-yyyy hh:mm:ss tt,dd.MM.yyyy hh:mm:ss tt,MM/dd/yyyy hh:mm:ss tt,yyyy/MM/dd hh:mm:ss tt,dd/MM/yyyy HH:mm,MM/dd/yyyy HH:mm,yyyy/MM/dd HH:mm,dd/MM/yyyy hh:mm tt,MM/dd/yyyy hh:mm tt,yyyy/MM/dd hh:mm tt,dd MMM yyyy HH:mm:ss,dd MMMM yyyy HH:mm:ss,yyyy MMM dd HH:mm:ss,dd/MM/yy HH:mm:ss,MM/dd/yy HH:mm:ss,yy/MM/dd HH:mm:ss,yyyyMMdd HHmmss,yyyy-MM-ddTHH:mm:ss,yyyy/MM/dd'T'HH:mm:ss,dd/MM/yyyy HH:mm:ss.SSS,MM/dd/yyyy HH:mm:ss.SSS,yyyy/MM/dd HH:mm:ss.SSS,dd-MMM-yyyy HH:mm:ss";

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
        public bool CallUnauthorizedApi(string serverAddr, string endpoint, string contentType, string method, dynamic Obj)
        {

            string uri = serverAddr + endpoint;

            var jsonObj = Newtonsoft.Json.JsonConvert.SerializeObject(Obj);

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(uri, UriKind.Absolute));

                httpWebRequest.ContentType = contentType;
                httpWebRequest.Method = method.ToUpper();
                httpWebRequest.Headers.Add("websecret", game_engine_key);

                //_logger.LogError("(Request) SecureService --> " + uri + " , Json : " + jsonObj);

                if (method == "GET")
                {
                    string html = string.Empty;
                    httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip;

                    using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        JObject JObject = JObject.Parse(reader.ReadToEnd());
                        //_logger.LogError("(Response) SecureService <-- " + uri + " , Json : " + JObject);

                        if (response.StatusCode == HttpStatusCode.OK)
                            return true;
                        else
                            return false;
                    }
                }

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonObj);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var text = streamReader.ReadToEnd();
                    JObject JObject = JObject.Parse(text);

                    //_logger.LogError("(Response) SecureService <-- " + uri + " , Json : " + JObject);

                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                        return true;
                    else
                        return false;
                }

            }
            catch (WebException webex)
            {
                WebResponse errResp = webex.Response;
                using (Stream respStream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);
                    string text = reader.ReadToEnd();
                    throw new Exception(text);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("(Request) SecureService --> " + uri + " , Error : " + ex.Message);

                throw ex;
            }
        }
    }
}