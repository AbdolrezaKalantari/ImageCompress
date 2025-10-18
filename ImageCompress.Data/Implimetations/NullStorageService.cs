using ImageCompress.Domain.Interfaces;

namespace ImageCompress.Data.Implimetations
{
    public sealed class NullStorageService : IStorageService
    {
        public Task<string> SaveAsync(string fileName, Stream content, string contentType, CancellationToken ct)
        {
            return Task.FromResult(string.Empty);
        }
    }
}
