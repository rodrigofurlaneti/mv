using System;
using System.Security.Cryptography;
using System.Text;

namespace Aplicacao.apipayzen
{
    public class SecurityHmacSha256
    {
        private static byte[] StringEncode(string text)
        {
            var encoding = new ASCIIEncoding();
            return encoding.GetBytes(text);
        }
        private static string HashEncode(byte[] hash)
        {
            return Convert.ToBase64String(hash);
        }
        private static byte[] HashHMAC(byte[] key, byte[] message)
        {
            var hash = new HMACSHA256(key);
            return hash.ComputeHash(message);
        }
        public static string HmacSha256(string stringToSign, string key)
        {
            return HashEncode(HashHMAC(StringEncode(key), StringEncode(stringToSign)));
        }
    }
}
