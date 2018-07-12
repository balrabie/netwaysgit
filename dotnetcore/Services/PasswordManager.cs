using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class PasswordManager
    {
        public byte[] Salt { get; private set; } = null;
        public string HashedPassword { get; private set; } = null;

        public PasswordManager(string password)
        {
            new RNGCryptoServiceProvider().GetBytes(Salt = new byte[16]);
            HashedPassword = GenerateHash(password);
        }

        public PasswordManager(string password, byte[] salt)
        {
            Salt = salt;
            HashedPassword = GenerateHash(password);
        }

        public string GenerateHash(string password)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, Salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(Salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20); // salt+hash

            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;
        }


        /// <summary>
        /// Assume we have a hashed password and a raw password.
        /// True is returned when the raw version of the hashed password is
        /// equal to the given password.
        /// In other words, the salt is filtered out of the hashed password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public bool CheckForMatch(string password)
        {
            // string HashedPassword = read from db ; // resolve this
            /* Extract the bytes */
            byte[] hashBytes = Convert.FromBase64String(HashedPassword);
            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            /* Compare the results */
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }

    }
}
