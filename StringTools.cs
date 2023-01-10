using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SharedClasses
{
    public static class StringTools
    {
        public static bool contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

        public static byte[] encode_SHA512(string input)
        {
            try
            {
                using (SHA512 sha = SHA512.Create())
                {
                    byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                    return hash;
                }
            }
            catch (Exception)
            {
                throw new Exception("AUTHENTICATION_COMPUTE_HASH_ERROR");
            }
        }
        private static string numericPart(string _reference, ref ushort _prefixEndIdx)
        {
            if (_reference == null)
                return null;
            string digits = "0123456789";
            _prefixEndIdx = (ushort)(_reference.Length - 1);
            while (_prefixEndIdx > 0 && digits.Contains(_reference.Substring(_prefixEndIdx, 1)) == true)
                --_prefixEndIdx;
            if (_prefixEndIdx == _reference.Length)
                return null;
            return _prefixEndIdx == 0 ? _reference : _reference.Substring(_prefixEndIdx + 1, _reference.Length - _prefixEndIdx - 1);
        }

        public static string nextReference(string _reference)
        {
            ushort prefixEndIdx = 0;
            string newReference = numericPart(_reference, ref prefixEndIdx);

            string _prefix = prefixEndIdx == 0 ? string.Empty : _reference.Substring(0, prefixEndIdx + 1);

            if (newReference == null)
                return null;
            long result;
            string nextRef;

            if (Int64.TryParse(newReference, out result))
            {
                nextRef = (result + 1).ToString();
                int deltaLength = newReference.Length - nextRef.Length;

                if (deltaLength > 0)
                    nextRef = new string('0', deltaLength) + nextRef;

                return _prefix + nextRef;

            }

            return null;
        }

        public static string StreamToString(Stream stream)
        {
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        public static MemoryStream StringToStream(string str)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(str);
            return new MemoryStream(byteArray);
        }

        public static Dictionary<string, string> decodeKeyValueString(string _keyValues, char equalIndictor, char keyValueSeparator)
        {
            char[] keyValues = _keyValues.ToCharArray();
            int eot = 0;
            int l = keyValues.Length;

            Dictionary<string, string> dict = new Dictionary<string, string>();

            while (eot < l)
            {
                string id = string.Empty;

                while (eot < l && keyValues[eot] != equalIndictor)
                {
                    id += keyValues[eot];
                    ++eot;
                }

                if (eot == l)
                    throw new Exception("parameters list format error");

                ++eot;

                string value = string.Empty;

                while (eot < l && keyValues[eot] != keyValueSeparator)
                {
                    value += keyValues[eot];
                    ++eot;
                }

                dict.Add(id, value);

                ++eot;
            }

            return dict;

        }

    }
}
