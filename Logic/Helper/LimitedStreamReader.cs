using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EtherBetClientLib.Helper
{
    class CroppedStream : Stream
    {
        private readonly Stream _baseStream;
        private long _length;
        private long _position;
        public CroppedStream(Stream baseStream, long length)
        {
            _baseStream = baseStream;
            _length = length;
            _position = 0;
        }


        public override void Flush()
        {
            _baseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_position == _length) return 0;
            if (_position + count > _length) count = (int)(_length - _position);
            return _baseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            _length = value;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override bool CanRead => _baseStream.CanRead;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => _length;
        public override long Position {
            get => _position;
            set => _position = value;
        }
    }
}
