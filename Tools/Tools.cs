using System;
using System.Text;

namespace Core
{
    public static class Tools
    {
        public static string GenerateRandomValue()
        {
            var guid = Guid.NewGuid().ToString().Replace("-", "");
            var random = new Random();
            var length = random.Next(6, 8);
            var password = "";
            for (var i = 0; i <= length; i++)
                password += guid.Substring(random.Next(1, guid.Length), 1);
            //
            return password;
        }

        public static string GenerateRandomCode(int length)
        {
            var code = "";
            var random = new Random();
            for (var i = 0; i < length; i++)
            {
                var randomNumber = random.Next(0, 9);
                code += randomNumber.ToString();
            }
            //
            return code;
        }

        public static string GetImageFromBase64(byte[] imgBytes)
        {
            return $"data:image/jpg;base64,{Convert.ToBase64String(imgBytes)}";
        }

        public static string ByteArrayToString(byte[] ba)
        {
            var hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}
