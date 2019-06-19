using System;

namespace GSM7BitConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            int test = '\r';
            string msg = "1234";
            while (true)
            {
                msg = Console.ReadLine();
                if (msg == "exit")
                    return;
                TestUnpack(msg);
            }
        }

        static void PackTest(string msg)
        {
            //Console.WriteLine("de convert to gsm: " + GSM7bitConverter.Gsm7BitPackHexToString(msg));
            Console.WriteLine("de convert to gsm pack: " + GSM7bitConverter.Gsm7BitPackHexToString(msg));
        }

        static void TestUnpack(string hex)
        {
            var bytes = hex.HexToByteArray();
            Console.WriteLine("de convert to gsm pack: " + GSM7bitConverter.UnpackByteArray(bytes).ToHexString());
        }
    }
}
