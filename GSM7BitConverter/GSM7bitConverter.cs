using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GSM7BitConverter
{
    public class GSM7bitConverter
    {

        public static char GetChar(int code)
        {
            if (_gsmToCharMap.ContainsKey(code))
                return _gsmToCharMap[code];
            return ' ';
        }

        public static int GetGsm(char c)
        {
            if (_charToGsmMap.ContainsKey(c))
                return _charToGsmMap[c];
            return 0x20;
        }

        public static byte GetEndByteOfChar(char c)
        {
            var code = GetGsm(c);
            var ret = (byte)(code & 0xff);
            return ret;
        }

        public static bool IsExtensionChar(char c)
        {
            return _extensionChars.Contains(c);
        }

        public static byte[] StringToGsm7Bit(string msg)
        {
            List<byte> bytes = new List<byte>();
            foreach (var c in msg)
            {
                if (IsExtensionChar(c))
                {
                    bytes.Add((byte)_extendedChar);
                }
                bytes.Add(GetEndByteOfChar(c));
            }
            return bytes.ToArray();
        }

        public static string StringToGsm7BitHex(string msg)
        {
            var bytes = StringToGsm7Bit(msg);
            return ByteArrayToHexString(bytes);
        }

        public static byte[] StringToGsm7BitPack(string msg)
        {
            var gsmBytes = StringToGsm7Bit(msg);

            byte[] answer = PackByteArray(gsmBytes);

            return answer;
        }

        public static string StringToGsm7BitPackHex(string msg)
        {
            var bytes = StringToGsm7BitPack(msg);
            return ByteArrayToHexString(bytes);
        }

        public static string Gsm7BitToString(byte[] bytes)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                var b = bytes[i];
                if ((int)b == _extendedChar)
                {
                    var n_b = bytes[i + 1];
                    var code = (b << 8) | n_b;
                    sb.Append(GetChar(code));
                    i++;
                }
                else
                {
                    sb.Append(GetChar(b));
                }
            }
            return sb.ToString();
        }

        public static string Gsm7BitHexToString(string hex)
        {
            return Gsm7BitToString(HexStringToByteArray(hex));
        }

        public static string Gsm7BitPackToString(byte[] bytes)
        {
            byte[] unpackBytes = UnpackByteArray(bytes);

            return Gsm7BitToString(unpackBytes);
        }

        public static string Gsm7BitPackHexToString(string hex)
        {
            return Gsm7BitPackToString(HexStringToByteArray(hex));
        }

        public static byte[] PackByteArray(byte[] gsmBytes)
        {
            List<byte> answer = new List<byte>();
            if (gsmBytes.Length == 1)
                answer.AddRange(gsmBytes);
            else
                for (int i = 0; i < gsmBytes.Length; i++)
                {
                    var b = gsmBytes[i];
                    var n_b = (byte)0;
                    if (i + 1 != gsmBytes.Length)
                        n_b = gsmBytes[i + 1];

                    var r_shift = i % 8;
                    var l_shift = 8 - r_shift - 1;

                    if (r_shift == 7) continue;

                    var r_byte = b >> r_shift;
                    var l_byte = n_b << l_shift;
                    var b_result = (byte)(r_byte | l_byte);
                    answer.Add(b_result);
                }
            return answer.ToArray();
        }

        public static byte[] UnpackByteArray(byte[] bytes)
        {
            List<byte> unpackBytes = new List<byte>();

            for (int i = 0; i < bytes.Length; i++)
            {
                var b = bytes[i];
                var p_b = (byte)0;
                
                var l_shift = i % 8;
                var r_shift = 8 - l_shift;
                var rm_shift = 8 - r_shift + 1;

                if (l_shift != 0)
                    p_b = bytes[i - 1];

                var r_b = (byte)(b << rm_shift);
                var rm_b = (byte)(r_b >> rm_shift << l_shift);
                var o_bit = p_b >> r_shift;
                var result_b = rm_b | o_bit;
                unpackBytes.Add((byte)result_b);

                if(l_shift == 6)
                {
                    var end_b = b >> 1;
                    if (end_b != 0)
                        unpackBytes.Add((byte)end_b);
                }
            }

            return unpackBytes.ToArray();
        }

        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        private static IDictionary<int, char> MakeGsmToCharMap(IDictionary<char, int> charToGsmMap)
        {
            var answer = new Dictionary<int, char>();
            foreach (var item in charToGsmMap)
            {
                answer.TryAdd(item.Value, item.Key);
            }
            return answer;
        }

        private static IDictionary<char, int> _charToGsmMap = new Dictionary<char, int>
        {
            {'@',   0x0},
            {'£',   0x1},
            {'$',   0x2},
            {'¥',   0x3},
            {'è',   0x4},
            {'é',   0x5},
            {'ù',   0x6},
            {'ì',   0x7},
            {'ò',   0x8},
            {'Ç',   0x9},
            {'\n',    0x0A},
            {'Ø',   0x0B},
            {'ø',   0x0C},
            {'\r',    0x0D},
            {'Å',   0x0E},
            {'å',   0x0F},
            {'Δ',   0x10},
            {'_',   0x11},
            {'Φ',   0x12},
            {'Γ',   0x13},
            {'Λ',   0x14},
            {'Ω',   0x15},
            {'Π',   0x16},
            {'Ψ',   0x17},
            {'Σ',   0x18},
            {'Θ',   0x19},
            {'Ξ',   0x1A},
            {'\u0027',    0x1B},
            {'\u000C',    0x1B0A},
            {'^',   0x1B14},
            {'{',   0x1B28},
            {'}',   0x1B29},
            {'\\',  0x1B2F},
            {'[',   0x1B3C},
            {'~',   0x1B3D},
            {']',   0x1B3E},
            {'|',   0x1B40},
            {'€',   0x1B65},
            {'Æ',   0x1C},
            {'æ',   0x1D},
            {'ß',   0x1E},
            {'É',   0x1F},
            {' ',   0x20},
            {'!',   0x21},
            {'"',   0x22},
            {'#',   0x23},
            {'¤',   0x24},
            {'%',   0x25},
            {'&',   0x26},
            {'\'',  0x27},
            {'(',   0x28},
            {')',   0x29},
            {'*',   0x2A},
            {'+',   0x2B},
            {',',   0x2C},
            {'-',   0x2D},
            {'.',   0x2E},
            {'/',   0x2F},
            {'0',   0x30},
            {'1',   0x31},
            {'2',   0x32},
            {'3',   0x33},
            {'4',   0x34},
            {'5',   0x35},
            {'6',   0x36},
            {'7',   0x37},
            {'8',   0x38},
            {'9',   0x39},
            {':',   0x3A},
            {';',   0x3B},
            {'<',   0x3C},
            {'=',   0x3D},
            {'>',   0x3E},
            {'?',   0x3F},
            {'¡',   0x40},
            {'A',   0x41},
            {'B',   0x42},
            {'C',   0x43},
            {'D',   0x44},
            {'E',   0x45},
            {'F',   0x46},
            {'G',   0x47},
            {'H',   0x48},
            {'I',   0x49},
            {'J',   0x4A},
            {'K',   0x4B},
            {'L',   0x4C},
            {'M',   0x4D},
            {'N',   0x4E},
            {'O',   0x4F},
            {'P',   0x50},
            {'Q',   0x51},
            {'R',   0x52},
            {'S',   0x53},
            {'T',   0x54},
            {'U',   0x55},
            {'V',   0x56},
            {'W',   0x57},
            {'X',   0x58},
            {'Y',   0x59},
            {'Z',   0x5A},
            {'Ä',   0x5B},
            {'Ö',   0x5C},
            {'Ñ',   0x5D},
            {'Ü',   0x5E},
            {'§',   0x5F},
            {'¿',   0x60},
            {'a',   0x61},
            {'b',   0x62},
            {'c',   0x63},
            {'d',   0x64},
            {'e',   0x65},
            {'f',   0x66},
            {'g',   0x67},
            {'h',   0x68},
            {'i',   0x69},
            {'j',   0x6A},
            {'k',   0x6B},
            {'l',   0x6C},
            {'m',   0x6D},
            {'n',   0x6E},
            {'o',   0x6F},
            {'p',   0x70},
            {'q',   0x71},
            {'r',   0x72},
            {'s',   0x73},
            {'t',   0x74},
            {'u',   0x75},
            {'v',   0x76},
            {'w',   0x77},
            {'x',   0x78},
            {'y',   0x79},
            {'z',   0x7A},
            {'ä',   0x7B},
            {'ö',   0x7C},
            {'ñ',   0x7D},
            {'ü',   0x7E},
            {'à',   0x7F},
        };

        private static IDictionary<int, char> _gsmToCharMap = MakeGsmToCharMap(_charToGsmMap);

        private static ISet<char> _extensionChars = new HashSet<char> { '\u000C', '^', '{', '}', '\\', '[', '~', ']', '|', '€' };

        private static int _extendedChar = 0x1b;
    }
}
