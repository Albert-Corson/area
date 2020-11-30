using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Dashboard.API.Common
{
    public static class Encryptor
    {
        public static string? Encrypt(string key, string text)
        {
            byte[] iv = new byte[16];

            using var aes = Aes.Create();
            if (aes == null)
                return null;
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using MemoryStream memoryStream = new MemoryStream();
            using CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            using (StreamWriter streamWriter = new StreamWriter(cryptoStream)) {
                streamWriter.Write(text);
            }

            byte[] array = memoryStream.ToArray();

            return Convert.ToBase64String(array);
        }
    }
}
