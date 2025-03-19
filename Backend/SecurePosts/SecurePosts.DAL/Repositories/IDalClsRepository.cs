using System;
using System.Collections.Generic;

namespace SecurePosts.DAL.Repositories
{
    public interface IDalClsRepository
    {
        public string Encrypt(string plainText);
        public string Decrypt(string cipherText);
        public string EncryptSha256Hash(string plainText);
        public DateTime ConvertToDateTime(string pDateValue);
    }
}
