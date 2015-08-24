// Author: Matt Ankerson
// Date: 23 August 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Globalization;
using System.Text;
using System.IO;

namespace ABLCloudStaff.Biz_Logic
{
    /// <summary>
    /// Provides utilities for Encryption and related services.
    /// </summary>
    public static class EncryptionUtilities
    {
        /// <summary>
        /// Generate and return a salt value
        /// </summary>
        /// <returns>Salt value in byte array format</returns>
        private static byte[] GenerateSaltValue()
        {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[Constants.SALT_SIZE];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number.
            return buff;
        }

        /// <summary>
        /// Hash and return a given password
        /// </summary>
        /// <param name="clearData">The plaintext password</param>
        /// <param name="saltValue">The value to append to the password for extra security</param>
        /// <param name="hash">The algorithm to use for the hash.</param>
        /// <returns></returns>
        public static string HashPassword(string plainText, byte[] saltValue=null)
        {
            SHA1 algorithm = SHA1.Create();

            // Convert text to a byte array and get a salt
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            if(saltValue == null)
            {
                // Generate a salt value
                saltValue = GenerateSaltValue();
            }

            // Create byte array to hold both the password and the salt.
            byte[] plainTextWithSalt = new byte[plainTextBytes.Length + saltValue.Length];

            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSalt[i] = plainTextBytes[i];
            for (int i = 0; i < saltValue.Length; i++)
                plainTextWithSalt[i] = saltValue[i];

            // Hash the whole thing
            byte[] hashed = algorithm.ComputeHash(plainTextWithSalt);

            // Return 'hashed', and store the salt used alongside it.

            return Convert.ToBase64String(hashed) + ":" + Convert.ToBase64String(saltValue);
        }

        /// <summary>
        /// Encrypt a given string to a byte array
        /// </summary>
        /// <param name="plainText">The text to encrypt</param>
        /// <param name="Key">Key, from Aes object</param>
        /// <param name="IV">IV, from Aes object</param>
        /// <returns>The encrypted text as binary.</returns>
        public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText is null or empty");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key is null or empty");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key is null or empty");

            byte[] encrypted;

            // Create an Aes object 
            // with the specified key and IV. 
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }

                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream. 
            return encrypted;
        }

        /// <summary>
        /// Decrypt a given binary stream and return the plaintext as string.
        /// </summary>
        /// <param name="cipherText">The binary to decrypt.</param>
        /// <param name="Key">Key, from the Aes object</param>
        /// <param name="IV">IV, from the Aes object</param>
        /// <returns>The plaintext.</returns>
        public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText is null or empty");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key is null or empty");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key is null or empty");

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an Aes object 
            // with the specified key and IV. 
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}