using ImageCompress.Domain.Entities;

namespace ImageCompress.Domain.Interfaces
{
    public interface IImageCompressor
    {
        Task<ImageCompressionResult> CompressAsync(ImageCompressionRequest request, CancellationToken ct);
    }
}
