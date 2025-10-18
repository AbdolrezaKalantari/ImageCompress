namespace ImageCompress.Domain.Entities
{
    public sealed class ImageCompressionRequest
    {
        public Stream InputStream { get; init; } = default!;
        public string FileName { get; init; } = default!;
        public string ContentType { get; init; } = default!;
        public int? Quality { get; init; }
        public bool AllowPngToJpegConversion { get; init; } = true;
        public ResizeOptions? Resize { get; init; }
        public long MaxInputBytes { get; init; }
    }
}
