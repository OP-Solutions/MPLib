using System;
using System.IO;

namespace EtherBetClientLib.Crypto.Signing
{
    class SignerStream : Stream
    {
        private readonly Stream _stream;
        private readonly byte[] _key;


        public SignerStream(Stream targetStream, byte[] key) : base()
        {
            _key = key;
            _stream = targetStream;
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead => _stream.CanRead;
        public override bool CanSeek => _stream.CanSeek;
        public override bool CanWrite => _stream.CanWrite;
        public override long Length => _stream.Length;
        public override long Position { get; set; }
    }
}
