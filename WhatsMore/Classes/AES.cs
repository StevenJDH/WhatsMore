using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace WhatsMore
{
    class AES
    {
        private string SaltKey { get; set; }

        public AES() => SaltKey = "Ì¥Ø¡eÈ"; // Hard-coded salt key for AES encryption.

        public string Encrypt(string clearText, string passKey)
        {
            byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);
            byte[] passBytes = Encoding.UTF8.GetBytes(passKey);
            byte[] saltBytes = getFrontLockSha(passKey, SaltKey); // The salt argument can be any data of at least 8 bytes.

            return Convert.ToBase64String(AESEncryptBytes(clearBytes, passBytes, saltBytes));
        }

        public string Decrypt(string cryptText, string passKey)
        {
            byte[] cryptBytes = Convert.FromBase64String(cryptText);
            byte[] passBytes = Encoding.UTF8.GetBytes(passKey);
            byte[] saltBytes = getFrontLockSha(passKey, SaltKey); // The salt argument can be any data of at least 8 bytes.

            return Encoding.UTF8.GetString(AESDecryptBytes(cryptBytes, passBytes, saltBytes));
        }

        private byte[] getFrontLockSha(string passKey, string saltKey)
        {
            string saltString = passKey + saltKey;

            // Convert text to bytes to get hash.
            ASCIIEncoding AE = new ASCIIEncoding();

            byte[] saltBuffer = AE.GetBytes(saltString);
            return GetSHA512(saltBuffer);
        }

        private byte[] GetSHA512(byte[] plainBuf)
        {
            byte[] hash;
            using (SHA512Managed hashVal = new SHA512Managed())
            {
                hash = hashVal.ComputeHash(plainBuf);
            }
            return hash;
        }

        private byte[] AESEncryptBytes(byte[] clearBytes, byte[] passBytes, byte[] saltBytes)
        {
            byte[] encryptedBytes = null;

            // Create a key from the password and salt, use 32K iterations.
            var key = new Rfc2898DeriveBytes(passBytes, saltBytes, 32768);

            // Create an AES object.
            using (Aes aes = new AesManaged())
            {
                // Set the key size to 256.
                aes.KeySize = 256;
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }
            return encryptedBytes;
        }

        private byte[] AESDecryptBytes(byte[] cryptBytes, byte[] passBytes, byte[] saltBytes)
        {
            byte[] clearBytes = null;

            // Create a key from the password and salt, use 32K iterations.
            var key = new Rfc2898DeriveBytes(passBytes, saltBytes, 32768);

            using (Aes aes = new AesManaged())
            {
                // Set the key size to 256.
                aes.KeySize = 256;
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cryptBytes, 0, cryptBytes.Length);
                        cs.Close();
                    }
                    clearBytes = ms.ToArray();
                }
            }
            return clearBytes;
        }
    }
}
