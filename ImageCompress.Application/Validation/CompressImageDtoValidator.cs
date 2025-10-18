using FluentValidation;
using ImageCompress.Application.Dtos;

namespace ImageCompress.Application.Validation
{
    public class CompressImageDtoValidator : AbstractValidator<CompressImageDto>
    {
        public CompressImageDtoValidator(long maxInputBytes)
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("File is required.")
                .Must(f => f.Length > 0).WithMessage("File is empty.")
                .Must(f => f.Length <= maxInputBytes)
                .WithMessage($"File exceeds max allowed size {maxInputBytes} bytes.");

            RuleFor(x => x.Quality)
                .InclusiveBetween(1, 100)
                .When(x => x.Quality.HasValue)
                .WithMessage("Quality must be between 1 and 100.");

            RuleFor(x => x.MaxWidth)
                .GreaterThan(0).When(x => x.MaxWidth.HasValue);

            RuleFor(x => x.MaxHeight)
                .GreaterThan(0).When(x => x.MaxHeight.HasValue);
        }
    }
}
