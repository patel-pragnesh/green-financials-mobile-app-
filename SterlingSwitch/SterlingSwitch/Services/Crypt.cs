using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SterlingSwitch.Services
{
    class Crypt
    {
    }

    public static class SHA
    {

        public static string GenerateSHA256String(string inputString)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha256.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        public static string GenerateSHA512String(string inputString)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
        public static string ComputeHMACSHA256(string jsonData, string  userid, string trasactionKey)

        {

            if (string.IsNullOrEmpty(jsonData))
                return string.Empty;
            if (string.IsNullOrEmpty(userid))
                return string.Empty;
            if (string.IsNullOrEmpty(trasactionKey))
                return string.Empty;

            var key = Encoding.UTF8.GetBytes(trasactionKey);

            var payload = Encoding.UTF8.GetBytes(string.Format("{0}^{1}", jsonData, userid));

            using (var hmacSHA = new HMACSHA256(key))

            {

                var hash = hmacSHA.ComputeHash(payload, 0, payload.Length);

                return BitConverter.ToString(hash).Replace("-", "").ToLower();

            }

        }

    }

    #region Encryption
    public static class Security
    {
        public static string GetKey(int length)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomNumber = new byte[length];
                rng.GetBytes(randomNumber);

                return Convert.ToBase64String(randomNumber);
            }


        }

        public static byte[] GetKeyByte(int length)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomNumber = new byte[length];
                rng.GetBytes(randomNumber);

                return randomNumber;
            }


        }
        #region Hashing
        static byte[] GetMessageByte(string msg)
        {
            return Encoding.UTF8.GetBytes(msg);
        }
        public static string MD5Hash(string message)
        {
            using (var md5 = MD5.Create())
            {
                var msgByte = GetMessageByte(message);

                var hash = md5.ComputeHash(msgByte);
                return Convert.ToBase64String(hash);
            }
        }
        public static string SHA256Hash(string message)
        {
            using (var md5 = SHA256.Create())
            {
                var msgByte = GetMessageByte(message);

                var hash = md5.ComputeHash(msgByte);
                return Convert.ToBase64String(hash);
            }
        }
        public static string SHA512Hash(string message)
        {
            using (var md5 = SHA512.Create())
            {
                var msgByte = GetMessageByte(message);

                var hash = md5.ComputeHash(msgByte);
                return Convert.ToBase64String(hash);
            }
        }
        #endregion

        #region HMAC
        public static string HmacSHA256(string message, byte[] key)
        {
            using (var sha256 = new HMACSHA256(key))
            {
                var msgByte = GetMessageByte(message);

                var hash = sha256.ComputeHash(msgByte);
                return Convert.ToBase64String(hash);
            }
        }
        public static string HmacSHA512(string message, byte[] key)
        {
            using (var sha512 = new HMACSHA512(key))
            {
                var msgByte = GetMessageByte(message);

                var hash = sha512.ComputeHash(msgByte);
                return Convert.ToBase64String(hash);
            }
        }
        #endregion

        #region Encryption with DES, TripleDES and AES
        #region DES
        public static byte[] DESEncrypt(byte[] dataToEncrypt, byte[] key, byte[] iv)
        {
            using (var des = new DESCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;
                des.Key = key;
                des.IV = iv;

                using (var memorystream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memorystream, des.CreateEncryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                    cryptoStream.FlushFinalBlock();
                    return memorystream.ToArray();
                }
            }
        }

        public static byte[] DESDecrypt(byte[] dataToDecrypt, byte[] key, byte[] iv)
        {
            using (var des = new DESCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;
                des.Key = key;
                des.IV = iv;

                using (var memorystream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memorystream, des.CreateDecryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                    cryptoStream.FlushFinalBlock();
                    return memorystream.ToArray();
                }
            }
        }

        #endregion

        #region TripleDES
        public static byte[] TripleDESEncrypt(byte[] dataToEncrypt, byte[] key, byte[] iv)
        {
            using (var des = new TripleDESCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;
                des.Key = key;
                des.IV = iv;

                using (var memorystream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memorystream, des.CreateEncryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                    cryptoStream.FlushFinalBlock();
                    return memorystream.ToArray();
                }
            }
        }

        public static byte[] TripleDESDecrypt(byte[] dataToDecrypt, byte[] key, byte[] iv)
        {
            using (var des = new TripleDESCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;
                des.Key = key;
                des.IV = iv;

                using (var memorystream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memorystream, des.CreateDecryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                    cryptoStream.FlushFinalBlock();
                    return memorystream.ToArray();
                }
            }
        }

        #endregion

        #region AES
        public static byte[] AesEncrypt(byte[] dataToEncrypt, byte[] key, byte[] iv)
        {
            using (var des = new AesCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;
                des.Key = key;
                des.IV = iv;

                using (var memorystream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memorystream, des.CreateEncryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                    cryptoStream.FlushFinalBlock();
                    return memorystream.ToArray();
                }
            }
        }

        public static byte[] AesDecrypt(byte[] dataToDecrypt, byte[] key, byte[] iv)
        {
            using (var des = new AesCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;
                des.Key = key;
                des.IV = iv;

                using (var memorystream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memorystream, des.CreateDecryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                    cryptoStream.FlushFinalBlock();
                    return memorystream.ToArray();
                }
            }
        }

        #endregion
        #endregion
    }
    #endregion
}
