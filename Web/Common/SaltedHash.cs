using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Web.Common
{
    public static class SaltedHash
    {
        private static readonly HashAlgorithm HashProvider;
        private static readonly int SaltLength;

        static SaltedHash()
        {
            // C# provides SHA1Managed, SHA256Managed, SHA384Managed, SHA512Managed and MD5CryptoServiceProvider
            HashProvider = new SHA256Managed();
            SaltLength = 4;
        }

        private static byte[] ComputeHash(byte[] data, byte[] salt)
        {
            // Allocate memory to store both the Data and Salt together
            byte[] dataAndSalt = new byte[data.Length + SaltLength];

            // Copy both the data and salt into the new array
            Array.Copy(data, dataAndSalt, data.Length);
            Array.Copy(salt, 0, dataAndSalt, data.Length, SaltLength);

            // Calculate the hash
            // Compute hash value of our plain text with appended salt.
            return HashProvider.ComputeHash(dataAndSalt);
        }

        public static void GetHashAndSalt(byte[] data, out byte[] hash, out byte[] salt)
        {
            // Allocate memory for the salt 
            salt = new byte[SaltLength];

            // Strong runtime pseudo-random number generator, on Windows uses CryptAPI
            // on Unix /dev/urandom
            var random = new RNGCryptoServiceProvider();

            // Create a random salt
            random.GetNonZeroBytes(salt);

            // Compute hash value of our data with the salt.
            hash = ComputeHash(data, salt);
        }

        public static void GetHashAndSaltString(string data, out string hash, out string salt)
        {
            byte[] hashOut;
            byte[] saltOut;

            // Obtain the Hash and Salt for the given string
            GetHashAndSalt(Encoding.UTF8.GetBytes(data), out hashOut, out saltOut);

            // Transform the byte[] to Base-64 encoded strings
            hash = Convert.ToBase64String(hashOut);
            salt = Convert.ToBase64String(saltOut);
        }

        public static bool VerifyHash(byte[] data, byte[] hash, byte[] salt)
        {
            byte[] newHash = ComputeHash(data, salt);

            //  No easy array comparison in C# -- we do the legwork
            if (newHash.Length != hash.Length) return false;

            return !hash.Where((t, lp) => !t.Equals(newHash[lp])).Any();
        }

        public static bool VerifyHashString(string data, string hash, string salt)
        {
            byte[] hashToVerify = Convert.FromBase64String(hash);
            byte[] saltToVerify = Convert.FromBase64String(salt);
            byte[] dataToVerify = Encoding.UTF8.GetBytes(data);
            return VerifyHash(dataToVerify, hashToVerify, saltToVerify);
        }
    }
}