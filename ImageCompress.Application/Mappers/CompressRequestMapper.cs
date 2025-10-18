using ImageCompress.Application.Dtos;
using ImageCompress.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace ImageCompress.Application.Mappers
{
    public static class CompressRequestMapper
    {
        public static ImageCompressionRequest Map(CompressImageDto dto, IConfiguration cfg)
        {
            return new ImageCompressionRequest
            {
                InputStream = dto.File.OpenReadStream(),
                FileName = dto.File.FileName,
                ContentType = dto.File.ContentType,
                Quality = dto.Quality ?? int.Parse(cfg["Compression:DefaultQuality"] ?? "70"),
                AllowPngToJpegConversion = dto.ConvertPngToJpegIfNoAlpha ?? true,
                MaxInputBytes = dto.File.Length,
                Resize = new ResizeOptions
                {
                    Enabled = dto.ResizeEnabled ?? true,
                    MaxWidth = dto.MaxWidth ?? int.Parse(cfg["Compression:Resize:MaxWidth"] ?? "1920"),
                    MaxHeight = dto.MaxHeight ?? int.Parse(cfg["Compression:Resize:MaxHeight"] ?? "1080")
                }
            };
        }
    }
}
