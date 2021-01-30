using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Area.API.Utilities
{
    public static class PasswordUtilities
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

        public static bool IsWeakPassword(
            string password,
            int requiredLength = 8,
            bool requireNonAlphanumeric = true,
            bool requireLowercase = true,
            bool requireUppercase = true,
            bool requireDigit = true)
        {
            if (!HasMinimumLength(password, requiredLength)) return true;
            if (requireNonAlphanumeric && !HasSpecialChar(password)) return true;
            if (requireLowercase && !HasLowerCaseLetter(password)) return true;
            if (requireUppercase && !HasUpperCaseLetter(password)) return true;
            if (requireDigit && !HasDigit(password)) return true;

            return false;
        }
 
        public static bool HasMinimumLength(string password, int minLength)
        {
            return password.Length >= minLength;
        }
 
        public static bool HasDigit(string password)
        {
            return password.Any(c => char.IsDigit(c));
        }
 
        public static bool HasSpecialChar(string password)
        {
            return password.IndexOfAny("!@#$%^&*?_~-£().,".ToCharArray()) != -1;
        }
 
        public static bool HasUpperCaseLetter(string password)
        {
            return password.Any(c => char.IsUpper(c));
        }
 
        public static bool HasLowerCaseLetter(string password)
        {
            return password.Any(c => char.IsLower(c));
        }
    }
}