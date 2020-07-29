using System;
using System.Buffers;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MPLib.Helper
{

    /// <summary>
    /// Stream reading writing helper class specially designed to make data exchange easier over network
    /// </summary>
    public static class StreamHelpers
    {

        public static async Task<short> ReadInt16Async(this Stream source)
        {
            var buffer = new byte[2];
            await source.ReadAsync(buffer, 0, 2);
            var len = (short)(buffer[0] | buffer[1] << 8);
            return len;
        }

        public static async Task WriteInt16Async(this Stream source, int value)
        {
            var buffer = new byte[2];
            if (value > short.MaxValue) throw new ArgumentOutOfRangeException(nameof(value));
            unchecked
            {
                buffer[0] = (byte)value;
                buffer[1] = (byte)(value >> 8);
                await source.WriteAsync(buffer, 0, 2);
            }
        }

        public static async Task<byte> ReadByteAsync(this Stream source)
        {
            var buffer = new byte[1];
            await source.ReadAsync(buffer, 0, 1);
            return buffer[0];
        }

        public static async Task<byte[]> ReadBytesOpaque8Async(this Stream source)
        {
            var len = await ReadByteAsync(source);
            return await source.ReadBlockAsync(len);
        }

        public static async Task<byte[]> ReadBytesOpaque16Async(this Stream source)
        {
            var len = await ReadInt16Async(source);
            return await source.ReadBlockAsync(len);
        }

        /// <summary>
        /// Reads block of bytes prefixed with 16bit integer length indicator
        /// </summary>
        /// <param name="bufferToWriteTo">buffer to which read bytes will be written</param>
        /// <param name="offset">offset to start writing at that index</param>
        /// <returns>bytes written</returns>
        public static async Task<int> ReadBytesOpaque16Async(this Stream source, byte[] bufferToWriteTo, int offset)
        {
            var len = await ReadInt16Async(source);
            await source.ReadBlockAsync(bufferToWriteTo, offset, len);
            return len;
        }

        public static async Task WriteBytesOpaque8Async(this Stream source, byte[] bytes)
        {
            if (bytes.Length > byte.MaxValue) throw new ArgumentOutOfRangeException(nameof(bytes));
            await WriteByteAsync(source, (byte)bytes.Length);
            await source.WriteAsync(bytes);
        }

        public static async Task WriteBytesOpaque16Async(this Stream source, byte[] bytes)
        {
            await WriteInt16Async(source, bytes.Length);
            await source.WriteAsync(bytes);
        }

        public static async Task WriteBytesOpaque16Async(this Stream source, byte[] bytes, int offset, int count)
        {
            await WriteInt16Async(source, count);
            await source.WriteAsync(bytes, offset, count);
        }

        public static  async Task WriteAsciiOpaque8Async(this Stream source, string value)
        {
            var bytes = Encoding.ASCII.GetBytes(value);
            await WriteBytesOpaque8Async(source, bytes);
        }

        public static async Task<string> ReadAsciiOpaque8Async(this Stream source)
        {
            var bytes = await ReadBytesOpaque8Async(source);
            return Encoding.ASCII.GetString(bytes);
        }

        public static async Task WriteByteAsync(this Stream source, byte value)
        {
            var buffer = new byte[1];
            buffer[0] = value;
            await source.WriteAsync(buffer, 0, 1);
        }


        public static async Task<T> ReadByteAsEnumAsync<T>(this Stream source) where T : Enum
        {
            var b = await ReadByteAsync(source);
            // ReSharper disable once PossibleInvalidCastException
            return (T)(object)b;
        }

        public static async Task WriteEnumAsByteAsync(this Stream source, Enum e)
        {
            await WriteByteAsync(source, (byte)(object)e);
        }
    }
}
