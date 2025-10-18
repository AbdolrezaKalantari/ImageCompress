using ImageCompress.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ImageCompress.Data.Implimetations
{
    public sealed class FileSystemStorageService : IStorageService
    {
        private readonly string _outputDir;
        public FileSystemStorageService(IConfiguration configuration)
        {
            _outputDir = configuration["Storage:FileSystem:OutputDir"] ?? "compressed";

            if (!Directory.Exists(_outputDir))
            {
                Directory.CreateDirectory(_outputDir);
            }
        }
        public async Task<string> SaveAsync(string fileName, Stream content, string contentType, CancellationToken ct)
        {
            var safeFileName = Path.GetFileName(fileName);
            var path = Path.Combine(_outputDir, safeFileName);

            var dir = Path.GetDirectoryName(path)!;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            content.Position = 0;
            using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            await content.CopyToAsync(fs, ct);
            return path;
        }
    }
}
