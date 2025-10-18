namespace ImageCompress.Domain.Entities
{
    public sealed class ResizeOptions
    {
        public bool Enabled { get; init; } = true;
        public int MaxWidth { get; init; } = 1920;
        public int MaxHeight { get; init; } = 1080;
    }
}
