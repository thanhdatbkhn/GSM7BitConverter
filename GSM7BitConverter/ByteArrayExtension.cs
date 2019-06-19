using System;
using System.Collections.Generic;
using System.Text;

namespace GSM7BitConverter
{
    public static class ByteArrayExtension
    {
        public static string ToHexString(this byte[] bytes)
        {
            return ByteArrayToHexString(bytes);
        }

        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
