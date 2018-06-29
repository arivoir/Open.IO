using System.IO;

namespace Open.IO
{
    public class StreamWithLength : StreamWrapper
    {
        private long? _length;
        public StreamWithLength(Stream stream, long? length)
            : base(stream)
        {
            _length = length;
        }

        public override long Length
        {
            get
            {
                return _length ?? base.Length;
            }
        }
    }
}
