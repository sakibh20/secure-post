using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace SecureService.DAL.Repositories
{
    public interface IDalClsRepository
    {
        public string Encrypt(string plainText);
        public string Decrypt(string cipherText);
        public string EncryptSha256Hash(string plainText);
        public DateTime ConvertToDateTime(string pDateValue);
        public bool CallUnauthorizedApi(string serverAddr, string endpoint, string contentType, string method, dynamic Obj);
    }
}
