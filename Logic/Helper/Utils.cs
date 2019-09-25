using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPR.Helper
{
    public static class Utils
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
    }
}
