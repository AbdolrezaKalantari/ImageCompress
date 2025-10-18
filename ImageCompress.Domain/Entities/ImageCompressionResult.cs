namespace ImageCompress.Domain.Entities
{
    public sealed class ImageCompressionResult
    {

        public Stream OutputStream { get; init; } = default!;
        public string OutputContentType { get; init; } = default!;
        public string OutputFileName { get; init; } = default!;
        public long InputBytes { get; init; }
        public long OutputBytes { get; init; }
        public string OriginalFormat { get; init; } = default!;
        public string OutputFormat { get; init; } = default!;
        public TimeSpan ProcessingTime { get; init; }
        public bool WasResized { get; init; }
        public bool WasConvertedFormat { get; init; }
    }
}
