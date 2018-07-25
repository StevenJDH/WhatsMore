/**
 * This file is part of WhatsMore <https://github.com/StevenJDH/WhatsMore>.
 * Copyright (C) 2018 Steven Jenkins De Haro.
 *
 * WhatsMore is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * WhatsMore is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with WhatsMore.  If not, see <http://www.gnu.org/licenses/>.
 */

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
        /// <summary>
        /// Encrypts text using AES 256-bit encryption and a salted password.
        /// </summary>
        /// <param name="clearText">Unencrypted text</param>
        /// <param name="passKey">Password to be used for encryption</param>
        /// <param name="saltKey">Salt to be used with password that can be any data of at least 8 bytes</param>
        /// <returns>Encrypted text</returns>
        public string Encrypt(string clearText, string passKey, string saltKey)
        {
            byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);
            byte[] passBytes = Encoding.UTF8.GetBytes(passKey);
            byte[] saltBytes = getFrontLockSha(passKey, saltKey);

            return Convert.ToBase64String(Encrypt(clearBytes, passBytes, saltBytes));
        }

        /// <summary>
        /// Decrypts text that was encrypted using AES 256-bit encryption and a salted password.
        /// </summary>
        /// <param name="cryptText">Encrypted text</param>
        /// <param name="passKey">Password that was used to encrypt text</param>
        /// <param name="saltKey">Salt that was used with password that is at least 8 bytes</param>
        /// <returns>Decrypted text</returns>
        public string Decrypt(string cryptText, string passKey, string saltKey)
        {
            byte[] cryptBytes = Convert.FromBase64String(cryptText);
            byte[] passBytes = Encoding.UTF8.GetBytes(passKey);
            byte[] saltBytes = getFrontLockSha(passKey, saltKey);

            return Encoding.UTF8.GetString(Decrypt(cryptBytes, passBytes, saltBytes));
        }

        /// <summary>
        /// Gets a SHA512 hash representation of the salted password in bytes.
        /// </summary>
        /// <param name="passKey">Encryption password</param>
        /// <param name="saltKey">Salt used with password</param>
        /// <returns>Hashed byte data from salted password</returns>
        private byte[] getFrontLockSha(string passKey, string saltKey)
        {
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            // Converts text to bytes and gets hash.
            byte[] saltedPaswordBytes = asciiEncoding.GetBytes(passKey + saltKey);

            return GetSHA512(saltedPaswordBytes);
        }

        /// <summary>
        /// Hashes byte data using the SHA512 cryptographic hashing function.
        /// </summary>
        /// <param name="plainData">Byte data to be hashed</param>
        /// <returns>Hashed byte data</returns>
        private byte[] GetSHA512(byte[] plainData)
        {
            byte[] hash = null;

            using (SHA512Managed hashVal = new SHA512Managed())
            {
                hash = hashVal.ComputeHash(plainData);
            }
            return hash;
        }

        /// <summary>
        /// Encrypts byte data using AES 256-bit encryption and a salted password.
        /// </summary>
        /// <param name="clearBytes">Unencrypted byte data</param>
        /// <param name="passBytes">Password to be used for encryption</param>
        /// <param name="saltBytes">Salt to be used with password that can be any data of at least 8 bytes</param>
        /// <returns>Encrypted byte data</returns>
        public byte[] Encrypt(byte[] clearBytes, byte[] passBytes, byte[] saltBytes)
        {
            byte[] encryptedBytes = null;

            // Creates a key from the password and salt using 32K iterations.
            var key = new Rfc2898DeriveBytes(passBytes, saltBytes, 32768);

            // Creates an Aes object not to be confused with this class.
            using (Aes aes = new AesManaged())
            {
                aes.KeySize = 256; // Set the key size to 256.
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

        /// <summary>
        /// Decrypts byte data that was encrypted using AES 256-bit encryption and a salted password.
        /// </summary>
        /// <param name="cryptBytes">Encrypted byte data</param>
        /// <param name="passBytes">Password that was used to encrypt byte data</param>
        /// <param name="saltBytes">Salt that was used with password that is at least 8 bytes</param>
        /// <returns>Decrypted byte data</returns>
        public byte[] Decrypt(byte[] cryptBytes, byte[] passBytes, byte[] saltBytes)
        {
            byte[] clearBytes = null;

            // Creates a key from the password and salt using 32K iterations.
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
