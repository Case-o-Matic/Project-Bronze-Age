using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Interaction
{
    // http://dotnet-snippets.de/snippet/encrypt-and-decrypt-strings/205
    public class Crypto
    {
        private static byte[] entropy = new byte[4] { 0x4d, 0x76, 0x61, 0x6e };

        public static string EncryptString(string clearText)
        {
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);
            byte[] encryptedData = EncryptString(clearBytes);

            return ASCIIEncoding.ASCII.GetString(encryptedData);
        }
        public static byte[] EncryptBytes(byte[] bytes)
        {
            return EncryptString(bytes);
        }

        public static byte[] DecryptBytes(byte[] bytes)
        {
            return DecryptString(bytes);
        }

        private static byte[] EncryptString(byte[] clearText)
        {
            return clearText; // Implement encryption
        }
        private static byte[] DecryptString(byte[] bytesText)
        {
            return bytesText; // Implement decryption
        }
    }
}
