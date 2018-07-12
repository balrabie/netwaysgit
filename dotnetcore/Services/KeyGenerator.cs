using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class KeyGenerator
    {
        public static string GetUniqueKey(int maxSize, string charPool)
        {
            int n = charPool.Count();
            char[] chars = new char[n];
            chars = charPool.ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (n)]);
            }
            return result.ToString();
        }

        public static string GetUniqueKeyMixed(int maxSize)
        {
            return GetUniqueKey(maxSize, "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890");
        }

        public static string GetUniqueKeyInteger(int maxSize)
        {
            return GetUniqueKey(maxSize, "1234567890");
        }
    }
}
