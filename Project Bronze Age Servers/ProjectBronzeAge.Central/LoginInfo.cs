using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;


namespace ProjectBronzeAge.Central
{
    public struct LoginInfo
    {
        private static readonly byte[] entropy = new byte[4] { 2, 0, 2, 7 };
        public string username, password;

        public LoginInfo(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public string EncryptPassword()
        {
            return Convert.ToBase64String(ProtectedData.Protect(ASCIIEncoding.ASCII.GetBytes(password), entropy, DataProtectionScope.LocalMachine));
        }
        public string DecryptPassword()
        {
            return Convert.ToBase64String(ProtectedData.Unprotect(ASCIIEncoding.ASCII.GetBytes(password), entropy, DataProtectionScope.LocalMachine));
        }
    }
}
