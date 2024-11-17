using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    internal class CryptoTools
    {
        public static string GenerateUniqueKey()
        {
            char[] Base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567".ToCharArray();

            const int length = 10; // 10 characters
            byte[] randomBytes = new byte[length];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes); // Fill the array with cryptographically secure random bytes
            }

            char[] secretCode = new char[length];

            for (int i = 0; i < length; i++)
            {
                secretCode[i] = Base32Chars[randomBytes[i] % Base32Chars.Length];
            }

            return new string(secretCode);
        }
    }
}
