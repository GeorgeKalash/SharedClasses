﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class TOTPGenerator
    {
        const int TIME_INTERVAL = 30;
        public static string GenerateTOTP(string _secret, int _secondsBack = 0)
        {
            var key = Base32Decode(_secret);
            var counter = GetCurrentCounter(TIME_INTERVAL, _secondsBack);

            // Convert counter to byte array in big-endian format
            var counterBytes = BitConverter.GetBytes(counter);
            Array.Reverse(counterBytes);  // Reverse to big-endian

            using (var hmac = new HMACSHA1(key))
            {
                var result = hmac.ComputeHash(counterBytes);
                int offset = result[result.Length - 1] & 0x0F;
                int binary = ((result[offset] & 0x7F) << 24)
                             | ((result[offset + 1] & 0xFF) << 16)
                             | ((result[offset + 2] & 0xFF) << 8)
                             | (result[offset + 3] & 0xFF);

                var otp = binary % 1000000;
                return otp.ToString("D6");
            }
        }

        private static byte[] Base32Decode(string base32)
        {
            var base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            var bytes = new byte[base32.Length * 5 / 8];

            int byteIndex = 0;
            int bits = 0;
            int bitsRemaining = 0;

            foreach (char c in base32)
            {
                int value = base32Chars.IndexOf(c);
                if (value < 0)
                    continue;

                bits = (bits << 5) | value;
                bitsRemaining += 5;

                if (bitsRemaining >= 8)
                {
                    bytes[byteIndex++] = (byte)(bits >> (bitsRemaining - 8));
                    bitsRemaining -= 8;
                }
            }

            return bytes;
        }

        private static long GetCurrentCounter(int _timeStep, int _secondsBack = 0)
        {
            var unixTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds - _secondsBack;
            return unixTimestamp / _timeStep;
        }
    }

}
