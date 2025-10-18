namespace ImageCompress.Domain.Interfaces
{
    public interface IStorageService
    {
        Task<string> SaveAsync(string fileName, Stream content, string contentType, CancellationToken ct);
    }
}
