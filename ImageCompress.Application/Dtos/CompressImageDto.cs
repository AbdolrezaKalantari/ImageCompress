using Microsoft.AspNetCore.Http;

namespace ImageCompress.Application.Dtos
{

    public sealed class CompressImageDto
    {
        public IFormFile File { get; set; } = default!;
        public int? Quality { get; set; } // 0..100
        public bool? ConvertPngToJpegIfNoAlpha { get; set; }
        public int? MaxWidth { get; set; }
        public int? MaxHeight { get; set; }
        public bool? ResizeEnabled { get; set; }
        public bool? ReturnAsLink { get; set; }
    }

}
