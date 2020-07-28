using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MPLib.Helper
{

    /// <summary>
    /// Stream reading writing helper class specially designed to make data exchange easier over network
    /// </summary>
    public class StreamController : Stream
    {
        public Stream BaseStream { get; set; }
        private readonly byte[] _buffer = new byte[8];


        public StreamController()
        {

        }

        public StreamController(Stream baseStream)
        {
            BaseStream = baseStream;
        }

        public async Task<short> ReadInt16Async()
        {
            await BaseStream.ReadAsync(_buffer, 0, 2);
            var len = (short)(_buffer[0] | _buffer[1] << 8);
            return len;
        }

        public async Task WriteInt16Async(int value)
        {
            if (value > short.MaxValue) throw new ArgumentOutOfRangeException(nameof(value));
            unchecked
            {
                _buffer[0] = (byte)value;
                _buffer[1] = (byte)(value >> 8);
                await BaseStream.WriteAsync(_buffer, 0, 2);
            }
        }

        public async Task<byte> ReadByteAsync()
        {
            await BaseStream.ReadAsync(_buffer, 0, 1);
            return _buffer[0];
        }

        public async Task<byte[]> ReadBytesOpaque8Async()
        {
            var len = await ReadByteAsync();
            return await BaseStream.ReadBlockAsync(len);
        }

        public async Task<byte[]> ReadBytesOpaque16Async()
        {
            var len = await ReadInt16Async();
            return await BaseStream.ReadBlockAsync(len);
        }

        /// <summary>
        /// Reads block of bytes prefixed with 16bit integer length indicator
        /// </summary>
        /// <param name="bufferToWriteTo">buffer to which read bytes will be written</param>
        /// <param name="offset">offset to start writing at that index</param>
        /// <returns>bytes written</returns>
        public async Task<int> ReadBytesOpaque16Async(byte[] bufferToWriteTo, int offset)
        {
            var len = await ReadInt16Async();
            await BaseStream.ReadBlockAsync(bufferToWriteTo, offset, len);
            return len;
        }

        public async Task WriteBytesOpaque8Async(byte[] bytes)
        {
            if (bytes.Length > byte.MaxValue) throw new ArgumentOutOfRangeException(nameof(bytes));
            await WriteByteAsync((byte)bytes.Length);
            await BaseStream.WriteAsync(bytes);
        }

        public async Task WriteBytesOpaque16Async(byte[] bytes)
        {
            await WriteInt16Async(bytes.Length);
            await BaseStream.WriteAsync(bytes);
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
            await BaseStream.WriteAsync(_buffer, 0, 1);
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

        public override void Flush()
        {
            BaseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return BaseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return BaseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            BaseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            BaseStream.Write(buffer, offset, count);
        }

        public override bool CanRead => BaseStream.CanRead;
        public override bool CanSeek => BaseStream.CanSeek;
        public override bool CanWrite => BaseStream.CanWrite;
        public override long Length => BaseStream.Length;
        public override long Position {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }
    }
}
