using ImageCompress.Domain.Entities;
using ImageCompress.Domain.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImageCompress.Data.Implimetations
{
    public sealed class ImageSharpImageCompressor : IImageCompressor
    {
        public async Task<ImageCompressionResult> CompressAsync(ImageCompressionRequest request, CancellationToken ct)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var inputBytes = request.InputStream.Length;

            request.InputStream.Position = 0;
            using var image = await Image.LoadAsync(request.InputStream, ct);

            var originalFormat = image.Metadata.DecodedImageFormat?.Name ?? "Unknown";           
            bool hasAlpha = image.PixelType.AlphaRepresentation != PixelAlphaRepresentation.None;
          
            bool wasResized = false;
            if (request.Resize?.Enabled == true)
            {
                if (image.Width > request.Resize.MaxWidth || image.Height > request.Resize.MaxHeight)
                {
                    image.Mutate(x => x.Resize(new SixLabors.ImageSharp.Processing.ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(request.Resize.MaxWidth, request.Resize.MaxHeight),
                        Sampler = KnownResamplers.Lanczos3
                    }));
                    wasResized = true;
                }
            }

           
            IImageEncoder encoder;
            string outputFormat;
            string outputContentType;
            bool convertedFormat = false;

            var targetQuality = request.Quality ?? 70;

            if (hasAlpha && originalFormat.Equals("PNG", StringComparison.OrdinalIgnoreCase))
            {
                encoder = new PngEncoder
                {
                    CompressionLevel = PngCompressionLevel.Level6,
                    FilterMethod = PngFilterMethod.Adaptive
                };
                outputFormat = "PNG";
                outputContentType = "image/png";
            }
            else
            {
               
                if (request.AllowPngToJpegConversion && !originalFormat.Equals("JPEG", StringComparison.OrdinalIgnoreCase))
                    convertedFormat = true;

                
                encoder = new JpegEncoder
                {
                    Quality = targetQuality
                };
                outputFormat = "JPEG";
                outputContentType = "image/jpeg";
            }

            var outputStream = new MemoryStream();
            await image.SaveAsync(outputStream, encoder, ct);
            outputStream.Position = 0;

            stopwatch.Stop();

            return new ImageCompressionResult
            {
                OutputStream = outputStream,
                OutputContentType = outputContentType,
                OutputFileName = BuildOutputFileName(request.FileName, outputFormat),
                InputBytes = inputBytes,
                OutputBytes = outputStream.Length,
                OriginalFormat = originalFormat,
                OutputFormat = outputFormat,
                ProcessingTime = stopwatch.Elapsed,
                WasResized = wasResized,
                WasConvertedFormat = convertedFormat
            };
        }

        private static string BuildOutputFileName(string originalName, string outFormat)
        {
            var name = Path.GetFileNameWithoutExtension(originalName);
            var ext = outFormat.Equals("PNG", StringComparison.OrdinalIgnoreCase) ? ".png" : ".jpg";
            return $"{name}_compressed{ext}";
        }
    }
}
