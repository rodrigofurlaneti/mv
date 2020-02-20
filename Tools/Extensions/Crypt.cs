using System;
using System.Configuration;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace Core.Extensions
{
    public class Crypt
    {
        public Crypt(string NewPass)
        {
            //Usa saltos para evitar atack de dicionário
            var pdb = new PasswordDeriveBytes(NewPass, SALTBYTEARRAY);
            //--------------------------
            /*Encoder tipo DES*/
            //DES.Create();
            //--------------------------
            /*Encoder tipo RC2*/
            //RC2.Create();
            //--------------------------
            /*Encoder tipo Rijndael*/
            //Rijndael.Create();
            //--------------------------
            /*Encoder tipo Triple-DES*/
            //TripleDES.Create();
            //--------------------------
            var algo = TripleDES.Create();
            //--------------------------
            MKEY = pdb.GetBytes(algo.KeySize / 8);
            MIV = pdb.GetBytes(algo.BlockSize / 8);
        }

        private byte[] MKEY = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
        private byte[] MIV = { 65, 110, 68, 26, 69, 178, 200, 219 };
        private byte[] SALTBYTEARRAY = { 0x49, 0x76, 0xee, 0x6e, 0x20, 0x4d, 0xff, 0x64, 0x76, 0x65, 0x64, 0x01, 0x76 };

        private byte[] EncryptDecrypt(byte[] inputBytes, bool encrpyt)
        {
            //--------------------------
            /*Encoder tipo DES*/
            //DES.Create();
            //--------------------------
            /*Encoder tipo RC2*/
            //RC2.Create();
            //--------------------------
            /*Encoder tipo Rijndael*/
            //Rijndael.Create();
            //--------------------------
            /*Encoder tipo Triple-DES*/
            //TripleDES.Create();
            //--------------------------
            var SA = TripleDES.Create();
            //--------------------------
            SA.Key = MKEY;
            SA.IV = MIV;
            //Transformação correta baseado no opção so usuario
            var transform = encrpyt ? SA.CreateEncryptor() : SA.CreateDecryptor();
            //Memory stream para saida
            var memStream = new MemoryStream();
            //Array de bytes para saida
            byte[] output;
            //Configura o encriptador e escreve no MemoryStream a saida
            var cryptStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write);
            //Escreve as informações no mecanismo do encriptador
            cryptStream.Write(inputBytes, 0, inputBytes.Length);
            //Finaliza e escreve todas as informações necessárias na memoria
            cryptStream.FlushFinalBlock();
            //Resgata o array de bytes
            output = memStream.ToArray();
            //Finaliza o mecanismo de criptografia e fecha o canal de comunicação de memoria
            cryptStream.Close();
            return output;
        }

        public static string DeCrypt(string texto)
        {
            return DeCrypt(texto, ConfigurationManager.AppSettings["CryptKey"]);
        }
        
        public static string DeCrypt(string texto, string pass)
        {
            var crypt = new Crypt(pass);
            return crypt.Decrypt(texto);
        }

        public static string EnCrypt(string texto)
        {
            return EnCrypt(texto, ConfigurationManager.AppSettings["CryptKey"]);
        }
        
        public static string EnCrypt(string texto, string pass)
        {
            var crypt = new Crypt(pass);
            return crypt.Encrypt(texto);
        }
        
        public string Encrypt(string inputText)
        {
            //Declarando encoder
            var UTF8Encoder = new UTF8Encoding();
            //Converte em base64 e envia para a função de encriptar e desencriptar depois retorna e converte para string novamente
            string Result = string.IsNullOrEmpty(inputText) ? string.Empty : Convert.ToBase64String(EncryptDecrypt(UTF8Encoder.GetBytes(inputText), true));
            //Retorno
            return Result;
        }

        public string Decrypt(string inputText)
        {
            //Declarando o encoder
            var utf8Encoder = new UTF8Encoding();
            try
            {
                //Converte em base64 e envia para a função de encriptar e desencriptar depois retorna e converte para string novamente
                return utf8Encoder.GetString(EncryptDecrypt(Convert.FromBase64String(inputText.Trim()), false));
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }

    public class CryptGeneric
    {
        public enum EnumCrypt
        {
            DES,
            RC2,
            Rijndael,
            TripleDES
        }

        public CryptGeneric(EnumCrypt typeCrypt, string NewPass)
        {
            //Usa saltos para evitar atack de dicionário
            var pdb = new PasswordDeriveBytes(NewPass, SALTBYTEARRAY);
            //--------------------------
            /*Encoder tipo DES*/
            //DES.Create();
            //--------------------------
            /*Encoder tipo RC2*/
            //RC2.Create();
            //--------------------------
            /*Encoder tipo Rijndael*/
            //Rijndael.Create();
            //--------------------------
            /*Encoder tipo Triple-DES*/
            //TripleDES.Create();
            //--------------------------

            SymmetricAlgorithm algo;
            switch (typeCrypt)
            {
                case EnumCrypt.DES:
                    algo = DES.Create();
                    break;
                case EnumCrypt.RC2:
                    algo = RC2.Create();
                    break;
                case EnumCrypt.Rijndael:
                    algo = Rijndael.Create();
                    break;
                case EnumCrypt.TripleDES:
                    algo = TripleDES.Create();
                    break;
                default:
                    throw new NotImplementedException($"Cryptation not implemented [{typeCrypt}].");
            }
            
            MKEY = pdb.GetBytes(algo.KeySize / 8);
            MIV = pdb.GetBytes(algo.BlockSize / 8);
        }

        private byte[] MKEY = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
        private byte[] MIV = { 65, 110, 68, 26, 69, 178, 200, 219 };
        private byte[] SALTBYTEARRAY = { 0x49, 0x76, 0xee, 0x6e, 0x20, 0x4d, 0xff, 0x64, 0x76, 0x65, 0x64, 0x01, 0x76 };

        private byte[] EncryptDecrypt(EnumCrypt typeCrypt, byte[] inputBytes, bool encrpyt)
        {
            //--------------------------
            /*Encoder tipo DES*/
            //DES.Create();
            //--------------------------
            /*Encoder tipo RC2*/
            //RC2.Create();
            //--------------------------
            /*Encoder tipo Rijndael*/
            //Rijndael.Create();
            //--------------------------
            /*Encoder tipo Triple-DES*/
            //TripleDES.Create();
            //--------------------------
            SymmetricAlgorithm SA;
            switch (typeCrypt)
            {
                case EnumCrypt.DES:
                    SA = DES.Create();
                    break;
                case EnumCrypt.RC2:
                    SA = RC2.Create();
                    break;
                case EnumCrypt.Rijndael:
                    SA = Rijndael.Create();
                    break;
                case EnumCrypt.TripleDES:
                    SA = TripleDES.Create();
                    break;
                default:
                    throw new NotImplementedException($"Cryptation not implemented [{typeCrypt}].");
            }

            //--------------------------
            SA.Key = MKEY;
            SA.IV = MIV;
            //Transformação correta baseado no opção so usuario
            var transform = encrpyt ? SA.CreateEncryptor() : SA.CreateDecryptor();
            //Memory stream para saida
            var memStream = new MemoryStream();
            //Array de bytes para saida
            byte[] output;
            //Configura o encriptador e escreve no MemoryStream a saida
            var cryptStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write);
            //Escreve as informações no mecanismo do encriptador
            cryptStream.Write(inputBytes, 0, inputBytes.Length);
            //Finaliza e escreve todas as informações necessárias na memoria
            cryptStream.FlushFinalBlock();
            //Resgata o array de bytes
            output = memStream.ToArray();
            //Finaliza o mecanismo de criptografia e fecha o canal de comunicação de memoria
            cryptStream.Close();
            return output;
        }

        public static string DeCrypt(EnumCrypt typeCrypt, string texto)
        {
            return DeCrypt(typeCrypt, texto, ConfigurationManager.AppSettings["CryptKey"]);
        }

        public static string DeCrypt(EnumCrypt typeCrypt, string texto, string pass)
        {
            var crypt = new CryptGeneric(typeCrypt, pass);
            return crypt.Decrypt(typeCrypt, texto);
        }

        public static string EnCrypt(EnumCrypt typeCrypt, string texto)
        {
            return EnCrypt(typeCrypt, texto, ConfigurationManager.AppSettings["CryptKey"]);
        }

        public static string EnCrypt(EnumCrypt typeCrypt, string texto, string pass)
        {
            var crypt = new CryptGeneric(typeCrypt, pass);
            return crypt.Encrypt(typeCrypt, texto);
        }

        public string Encrypt(EnumCrypt typeCrypt, string inputText)
        {
            //Declarando encoder
            var UTF8Encoder = new UTF8Encoding();
            //Converte em base64 e envia para a função de encriptar e desencriptar depois retorna e converte para string novamente
            string Result = Convert.ToBase64String(EncryptDecrypt(typeCrypt, UTF8Encoder.GetBytes(inputText), true));
            //Retorno
            return Result;
        }

        public string Decrypt(EnumCrypt typeCrypt, string inputText)
        {
            //Declarando o encoder
            var utf8Encoder = new UTF8Encoding();
            try
            {
                //Converte em base64 e envia para a função de encriptar e desencriptar depois retorna e converte para string novamente
                return utf8Encoder.GetString(EncryptDecrypt(typeCrypt, Convert.FromBase64String(inputText.Trim()), false));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

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
