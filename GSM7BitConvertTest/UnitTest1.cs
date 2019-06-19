using GSM7BitConverter;
using System;
using Xunit;

namespace GSM7BitConvertTest
{
    public class UnitTest1
    {
        [Fact]
        public void TestGetCharFromGSM()
        {
            Assert.True('1' == GSM7bitConverter.GetChar(0x31), "wrong convert char 1");
            Assert.True('<' == GSM7bitConverter.GetChar(0x3c), "wrong convert char <");
            Assert.True('N' == GSM7bitConverter.GetChar(0x4e), "wrong convert char N");
            Assert.True('g' == GSM7bitConverter.GetChar(0x67), "wrong convert char g");

            Assert.True('+' == GSM7bitConverter.GetChar(0x2b), "wrong convert char +");
            Assert.True('/' == GSM7bitConverter.GetChar(0x2f), "wrong convert char /");

            Assert.True('^' == GSM7bitConverter.GetChar(0x1b14), "wrong convert char ^");
            Assert.True('[' == GSM7bitConverter.GetChar(0x1b3c), "wrong convert char [");
            Assert.True('|' == GSM7bitConverter.GetChar(0x1b40), "wrong convert char |");
            Assert.True(' ' == GSM7bitConverter.GetChar(0x20), "wrong convert char space");
        }

        [Fact]
        public void TestGetGsmFromChar()
        {
            Assert.True(0x31 == GSM7bitConverter.GetGsm('1'), "wrong convert char 1");
            Assert.True(0x3c == GSM7bitConverter.GetGsm('<'), "wrong convert char <");
            Assert.True(0x4e == GSM7bitConverter.GetGsm('N'), "wrong convert char N");
            Assert.True(0x67 == GSM7bitConverter.GetGsm('g'), "wrong convert char g");

            Assert.True(0x2b == GSM7bitConverter.GetGsm('+'), "wrong convert char +");
            Assert.True(0x2f == GSM7bitConverter.GetGsm('/'), "wrong convert char /");

            Assert.True(0x1b14 == GSM7bitConverter.GetGsm('^'), "wrong convert char ^");
            Assert.True(0x1b3c == GSM7bitConverter.GetGsm('['), "wrong convert char [");
            Assert.True(0x1b40 == GSM7bitConverter.GetGsm('|'), "wrong convert char |");
            Assert.True(0x20 == GSM7bitConverter.GetGsm(' '), "wrong convert char space");
        }

        [Fact]
        public void TestEncodeGsm7Bit()
        {
            Assert.True("313233343536373839" == GSM7bitConverter.StringToGsm7BitHex("123456789"), "wrong convert 123456789");

            Assert.True("1b14" == GSM7bitConverter.StringToGsm7BitHex("^"), "wrong convert ^");
            Assert.True("1b141b281b291b2f" == GSM7bitConverter.StringToGsm7BitHex("^{}\\"), "wrong convert ^{}\\");
        }

        [Fact]
        public void TestDecodeGsm7Bit()
        {
            Assert.True("123456789" == GSM7bitConverter.Gsm7BitHexToString("313233343536373839"), "wrong convert 123456789");

            Assert.True("^" == GSM7bitConverter.Gsm7BitHexToString("1b14"), "wrong convert ^");
            Assert.True("^{}\\" == GSM7bitConverter.Gsm7BitHexToString("1b141b281b291b2f"), "wrong convert ^{}\\");
        }

        [Fact]
        public void TestEncodeGsm7BitPack()
        {
            Assert.True("31".ToLower() == GSM7bitConverter.StringToGsm7BitPackHex("1"), "wrong convert 1");
            Assert.True("3119".ToLower() == GSM7bitConverter.StringToGsm7BitPackHex("12"), "wrong convert 12");
            Assert.True("31D98C56B3DD7039".ToLower() == GSM7bitConverter.StringToGsm7BitPackHex("123456789"), "wrong convert 123456789");

            Assert.True("1B0A".ToLower() == GSM7bitConverter.StringToGsm7BitPackHex("^"), "wrong convert ^");
            Assert.True("1BCA06B5496D5E".ToLower() == GSM7bitConverter.StringToGsm7BitPackHex("^{}\\"), "wrong convert ^{}\\");
        }
    }
}
