using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using EtherBetClientLib.Networking;

namespace EtherBetClientLib.Helper
{

    /// <summary>
    /// Stream reading writing helper class specially designed to make data exchange easier over network
    /// </summary>
    public class StreamController
    {
        private readonly Stream _baseStream;
        private readonly byte[] _buffer;


        public StreamController(Stream baseStream)
        {
            _baseStream = baseStream;
            _buffer = new byte[8];
        }

        public async Task<byte[]> ReadBytesOpaque8Async()
        {
            var len = await ReadByteAsync();
            return await _baseStream.ReadBlockAsync(len);
        }

        public async Task<byte[]> ReadBytesOpaque16Async()
        {
            await _baseStream.ReadAsync(_buffer, 0, 2);
            var len = (short)(_buffer[0] | _buffer[1] << 8);
            return await _baseStream.ReadBlockAsync(len);
        }

        public async Task WriteBytesOpaque8Async(byte[] bytes)
        {
            if (bytes.Length > byte.MaxValue) throw new ArgumentOutOfRangeException(nameof(bytes));
            await WriteByteAsync((byte)bytes.Length);
            await _baseStream.WriteAsync(bytes);
        }

        public async Task WriteBytesOpaque16Async(byte[] bytes)
        {
            if (bytes.Length > short.MaxValue) throw new ArgumentOutOfRangeException(nameof(bytes));
            unchecked
            {
                var len = bytes.Length;
                _buffer[0] = (byte)len;
                _buffer[1] = (byte)(len >> 8);
                await _baseStream.WriteAsync(_buffer, 0, 2);
                await _baseStream.WriteAsync(bytes);
            }

        }

        public async Task WriteAsciiOpaque8Async(string value)
        {
            var bytes = Encoding.ASCII.GetBytes(value);
            await WriteBytesOpaque8Async(bytes);
        }

        public async Task<string> ReadAsciiOpaque8Async()
        {
            var bytes = await ReadBytesOpaque8Async();
            return Encoding.ASCII.GetString(bytes);
        }

        public async Task WriteByteAsync(byte value)
        {
            _buffer[0] = value;
            await _baseStream.WriteAsync(_buffer, 0, 1);
        }

        public async Task<byte> ReadByteAsync()
        {
            await _baseStream.ReadAsync(_buffer, 0, 1);
            return _buffer[0];
        }

        public async Task<T> ReadByteAsEnumAsync<T>() where T : Enum
        {
            var b = await ReadByteAsync();
            // ReSharper disable once PossibleInvalidCastException
            return (T)(object)b;
        }

        public async Task WriteEnumAsByteAsync(Enum e)
        {
            await WriteByteAsync((byte)(object)e);
        }
    }
}
