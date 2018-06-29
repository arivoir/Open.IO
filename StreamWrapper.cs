using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Open.IO
{
    public class StreamWrapper : Stream
    {
        public Stream InnerStream { get; private set; }

        public StreamWrapper(Stream stream)
        {
            InnerStream = stream;
        }

        public override bool CanRead
        {
            get
            {
                return InnerStream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return InnerStream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return InnerStream.CanWrite;
            }
        }

        public bool TryGetPosition(out long? position)
        {
            if (!InnerStream.CanSeek)
            {
                position = null;
                return false;
            }
            try
            {
                position = InnerStream.Position;
                return true;
            }
            catch
            {
                position = null;
                return false;
            }
        }

        public override long Length
        {
            get
            {
                return InnerStream.Length;
            }
        }

        public override long Position
        {
            get
            {
                return InnerStream.Position;
            }

            set
            {
                InnerStream.Position = value;
            }
        }

        public override void Flush()
        {
            InnerStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return InnerStream.Read(buffer, offset, count);
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return await InnerStream.ReadAsync(buffer, offset, count, cancellationToken);
        }

        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            await base.WriteAsync(buffer, offset, count, cancellationToken);
        }

        public override async Task FlushAsync(CancellationToken cancellationToken)
        {
            await InnerStream.FlushAsync(cancellationToken);
        }

        public override async Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            await InnerStream.CopyToAsync(destination, bufferSize, cancellationToken);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return InnerStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            InnerStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            InnerStream.Write(buffer, offset, count);
        }
    }
}
