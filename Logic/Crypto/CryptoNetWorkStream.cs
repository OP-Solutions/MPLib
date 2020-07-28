using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace MPLib.Crypto
{
    public class CryptoNetWorkStream : Stream
    {

        private Stream _inStream;
        private Stream _senderStream;
        private Stream _recieverStream;

        public CryptoNetWorkStream(Stream baseStream, SymmetricAlgorithm aes)
        {
            _senderStream = new CryptoStream(_inStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            _recieverStream = new CryptoStream(_inStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            _inStream = baseStream;
        }

        public override void Flush()
        {
            _senderStream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _recieverStream.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _senderStream.Write(buffer, offset, count);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return _senderStream.WriteAsync(buffer, offset, count, cancellationToken);
        }

        public override bool CanRead { get; } = true;
        public override bool CanSeek { get; } = false;
        public override bool CanWrite { get; } = true;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }
    }
}
