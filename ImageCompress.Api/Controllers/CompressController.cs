using ImageCompress.Application.Dtos;
using ImageCompress.Application.Mappers;
using ImageCompress.Application.Validation;
using ImageCompress.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ImageCompress.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class CompressController : ControllerBase
    {
        private readonly IImageCompressor _compressor;
        private readonly IStorageService _storage;
        private readonly IConfiguration _cfg;
        private readonly ILogger<CompressController> _logger;

        public CompressController(IImageCompressor compressor, IStorageService storage, IConfiguration cfg, ILogger<CompressController> logger)
        {
            _compressor = compressor;
            _storage = storage;
            _cfg = cfg;
            _logger = logger;
        }

        [HttpPost]
        [RequestSizeLimit(10_000_000)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Post([FromForm] CompressImageDto dto, CancellationToken ct)
        {
            var maxBytes = long.Parse(_cfg["Compression:MaxInputBytes"] ?? "5242880");
            var validator = new CompressImageDtoValidator(maxBytes);
            var validation = validator.Validate(dto);
            if (!validation.IsValid)
                return BadRequest(new { errors = validation.Errors.Select(e => e.ErrorMessage) });

            if (!IsSupportedContentType(dto.File.ContentType))
                return BadRequest("Only JPG and PNG are supported.");

            var request = CompressRequestMapper.Map(dto, _cfg);
            var result = await _compressor.CompressAsync(request, ct);

            _logger.LogInformation("Compressed {File} from {In} bytes to {Out} bytes in {Ms} ms",
                dto.File.FileName, result.InputBytes, result.OutputBytes, result.ProcessingTime.TotalMilliseconds);

            var returnAsLink = dto.ReturnAsLink ?? true;

            if (returnAsLink)
            {
                var path = await _storage.SaveAsync(result.OutputFileName, result.OutputStream, result.OutputContentType, ct);
                var publicUrl = Url.Content($"~/{Path.GetFileName(path)}");
                return Ok(new
                {
                    url = publicUrl,
                    inputBytes = result.InputBytes,
                    outputBytes = result.OutputBytes,
                    ratio = Math.Round((double)result.OutputBytes / result.InputBytes, 3),
                    durationMs = result.ProcessingTime.TotalMilliseconds,
                    format = result.OutputFormat
                });
            }
            else
            {
                return File(result.OutputStream, result.OutputContentType, result.OutputFileName);
            }
        }

        private static bool IsSupportedContentType(string? ct) =>
            ct is "image/jpeg" or "image/jpg" or "image/png";
    }
}
