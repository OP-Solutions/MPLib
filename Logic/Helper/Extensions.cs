using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EtherBetClientLib.Helper
{
    public static class Extensions
    {
        public static byte[] FromHex(this string hexStr)
        {
            if (hexStr.StartsWith("0x")) hexStr = hexStr.Substring(2);
            if (hexStr.Length % 2 != 0) throw new ArgumentException("input string is not valid hex", nameof(hexStr));
            var result = new byte[hexStr.Length / 2];
            for (int i = 0; i < hexStr.Length - 1; i += 2)
            {
                result[i / 2] = byte.Parse(hexStr.Substring(i, 2));
            }

            return result;
        }


        public static string ToHex(this byte[] bytes)
        {
            var result = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
            {
                result.AppendFormat("{0:x2}", b);
            }

            return result.ToString();
        }

        public static void ReadBlock(this Stream stream, byte[] buffer, int offset, int count)
        {
            var receivedCount = offset;
            while (receivedCount < count)
            {
                receivedCount += stream.Read(buffer, receivedCount, count - receivedCount);
            }
        }


        public static byte[] ReadBlock(this Stream stream, int countToRead)
        {
            var receiveBuffer = new byte[countToRead];
            var receivedCount = 0;
            while (receivedCount < countToRead)
            {
                receivedCount += stream.Read(receiveBuffer, receivedCount, countToRead - receivedCount);
            }

            return receiveBuffer;
        }

        public static async Task<byte[]> ReadBlockAsync(this Stream stream, int countToRead)
        {
            var receiveBuffer = new byte[countToRead];
            var receivedCount = 0;
            while (receivedCount < countToRead)
            {
                receivedCount += await stream.ReadAsync(receiveBuffer, receivedCount, countToRead - receivedCount);
            }

            return receiveBuffer;
        }


        public static async Task ReadBlockAsync(this Stream stream, byte[] buffer, int offset, int count)
        {
            var receivedCount = offset;
            while (receivedCount < count)
            {
                receivedCount += await stream.ReadAsync(buffer, receivedCount, count - receivedCount);
            }
        }

    }
}
