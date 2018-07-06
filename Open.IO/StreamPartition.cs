using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Open.IO
{
    /// <summary>
    /// The stream exposes part of another stream.
    /// </summary>
    public class StreamPartition : StreamWrapper
    {
        private long _offset, _length, _position;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamPartition"/> class.
        /// </summary>
        /// <param name="stream">The original stream.</param>
        /// <param name="offset">The offset where this partition starts.</param>
        /// <param name="length">The length of the partition.</param>
        public StreamPartition(Stream stream, long offset, long length)
            : base(stream)
        {
            _offset = offset;
            _length = length;
        }

        public override long Length
        {
            get
            {
                return _length;
            }
        }

        public override long Position
        {
            get
            {
                return _position;
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            count = Math.Min((int)(Length - Position), count);
            var readBytes = await base.ReadAsync(buffer, offset, count, cancellationToken);
            _position += readBytes;
            return readBytes;
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
