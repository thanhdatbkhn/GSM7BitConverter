using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSM7BitConverter
{
    public static class StringExtension
    {
        public static byte[] HexToByteArray(this string s)
        {
            return HexStringToByteArray(s);
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
