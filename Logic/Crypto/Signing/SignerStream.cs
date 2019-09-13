using System;
using System.IO;
using System.Text;
using Nethereum.Signer;
using Context = AppContext.Context;

namespace SPR.Crypto.Signing
{
    class SignerStream : Stream
    {
        private Stream _stream;
        private readonly MessageSigner _signer = new MessageSigner();

        public SignerStream(Stream targetStream) : base()
        {
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
            var buf = new byte[count];
            Array.Copy(buffer, offset, buf, 0, count);
            var signedMessage = _signer.HashAndSign(buf, Context.WalletKey);
            var bytes = Encoding.ASCII.GetBytes(signedMessage);
            _stream.Write(buffer, offset, count);
            _stream.Write(bytes, 0, bytes.Length);
        }

        public override bool CanRead => _stream.CanRead;
        public override bool CanSeek => _stream.CanSeek;
        public override bool CanWrite => _stream.CanWrite;
        public override long Length => _stream.Length;
        public override long Position { get; set; }
    }
}
