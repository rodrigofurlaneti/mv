using System;
using System.Configuration;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace ApiInfox.Helpers
{
    public class CryptoApiInfox
    {
        public static string Encrypt(string originalString, string password)
        {
            var bytes = Encoding.ASCII.GetBytes(password);
            var cryptoProvider = new DESCryptoServiceProvider
            {
                Mode = CipherMode.ECB,
                Padding = PaddingMode.Zeros,
                Key = bytes
            };

            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(), CryptoStreamMode.Write);
            var writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();

            var arrayByte = memoryStream.GetBuffer();

            return BitConverter.ToString(arrayByte, 0, (int)memoryStream.Length).Replace("-", "");
        }

        public static string EncryptApi(string field, string password)
        {
            return Encrypt($"{field.Length.ToString("D4")}{field}", password);
        }

        public static string DecryptApi(string field, string password)
        {
            var dado = Decrypt(field, password);
            return dado.Substring(4, Convert.ToInt32(dado.Substring(0, 4)));
        }

        public static string Decrypt(string cryptedString, string password)
        {
            if (string.IsNullOrEmpty(cryptedString))
            {
                throw new ArgumentNullException
                   ("The string which needs to be decrypted can not be null.");
            }

            var bytes = Encoding.ASCII.GetBytes(password);
            var cryptoProvider = new DESCryptoServiceProvider
            {
                Mode = CipherMode.ECB,
                Padding = PaddingMode.Zeros,
                Key = bytes
            };

            if (!cryptedString.Contains("-"))
            {
                var arrayByteString = new List<string>();
                for (int i = 0; i < cryptedString.Length; i += 2)
                {
                    arrayByteString.Add(cryptedString.Substring(i, 2));
                }
                cryptedString = string.Join("-", arrayByteString.ToArray());
            }

            var arrayByte = Array.ConvertAll(cryptedString.Split('-'), s => Convert.ToByte(s, 16));
            
            using (MemoryStream stream = new MemoryStream(arrayByte, 0, arrayByte.Length))
            {
                using (CryptoStream cs = new CryptoStream(stream, cryptoProvider.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs, Encoding.ASCII))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }
}
