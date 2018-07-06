using System.IO;

namespace Open.IO
{
    /// <summary>
    /// This stream takes another stream, one whose lenght it is not available, and a lenght parameter.
    /// </summary>
    /// <remarks>
    /// This stream is useful to wrap network streams whose content is specified as a response header and allow readers to show the progress of the download.
    /// </remarks>
    public class StreamWithLength : StreamWrapper
    {
        private long? _length;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamWithLength"/> class.
        /// </summary>
        /// <param name="stream">The original stream.</param>
        /// <param name="length">The length of the stream.</param>
        public StreamWithLength(Stream stream, long? length)
            : base(stream)
        {
            _length = length;
        }

        /// <summary>
        /// Gets the length of the stream.
        /// </summary>
        public override long Length
        {
            get
            {
                return _length ?? base.Length;
            }
        }
    }
}
