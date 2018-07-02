using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Open.IO
{
    public static class StreamEx
    {
        public static async Task<byte[]> ReadAsBufferAsync(this Stream stream, CancellationToken cancellationToken, IProgress<StreamProgress> progress = null, int chunkSize = 65536)
        {
            var length = stream.GetLength();
            byte[] buffer = null;
            if (length.HasValue)
                buffer = new byte[length.Value];
            var buffers = new List<byte[]>();
            int offset = 0;
            int readBytes = 1;
            while (readBytes > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (length.HasValue)
                {
                    readBytes = await stream.ReadAsync(buffer, offset, Math.Min((int)(length - offset), chunkSize));
                }
                else
                {
                    buffer = new byte[chunkSize];
                    readBytes = await stream.ReadAsync(buffer, 0, chunkSize);
                    if (readBytes < chunkSize)
                        Array.Resize(ref buffer, readBytes);
                    buffers.Add(buffer);
                }
                offset += readBytes;
                if (length.HasValue)
                    progress?.Report(new StreamProgress(offset, length.Value));
            }
            if (length.HasValue)
                return buffer;
            else
                return buffers.SelectMany(b => b).ToArray();
        }

        public static async Task ReadToEndAsync(this Stream stream, CancellationToken cancellationToken, IProgress<StreamProgress> progress = null, int bufferSize = 65536)
        {
            var length = stream.GetLength();
            var buffer = new byte[bufferSize];
            var offset = stream.Position;
            int readBytes = 1;
            while (readBytes > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var bytesToRead = Math.Min((int)(length.HasValue ? length - offset : int.MaxValue), bufferSize);
                readBytes = await stream.ReadAsync(buffer, 0, bytesToRead);
                offset += readBytes;
                if (length.HasValue)
                    progress?.Report(new StreamProgress(offset, length.Value));
            }
        }

        public static async Task CopyToAsync(this Stream stream, Stream destination, int bufferSize = 65536, bool flush = false, IProgress<StreamProgress> progress = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var length = stream.GetLength();
            var buffer = new byte[bufferSize];
            int offset = 0;
            int readBytes = 1;
            while (readBytes > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var bytesToRead = Math.Min((int)(length.HasValue ? length - offset : int.MaxValue), bufferSize);
                readBytes = await stream.ReadAsync(buffer, 0, bytesToRead);
                await destination.WriteAsync(buffer, 0, readBytes);
                if (flush)
                    await destination.FlushAsync(cancellationToken);
                offset += readBytes;
                if (length.HasValue)
                    progress?.Report(new StreamProgress(offset, length.Value));
            }
        }

        public static long? GetLength(this Stream stream)
        {
            try
            {
                return stream.Length;
            }
            catch { }
            return null;
        }
    }

    public class StreamProgress
    {
        public StreamProgress(long bytes, long total)
        {
            Bytes = bytes;
            TotalBytes = total;
        }

        public long? TotalBytes { get; private set; }

        public long Bytes { get; private set; }
    }

}
