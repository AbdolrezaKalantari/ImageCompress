using ImageCompress.Data.Implimetations;
using ImageCompress.Domain.Interfaces;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IImageCompressor, ImageSharpImageCompressor>();


var storageMode = builder.Configuration["Storage:Mode"] ?? "FileSystem";
if (storageMode.Equals("FileSystem", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddScoped<IStorageService, FileSystemStorageService>();
}
else
{
    builder.Services.AddScoped<IStorageService, NullStorageService>();
}

var app = builder.Build();


var outputDir = builder.Configuration["Storage:FileSystem:OutputDir"] ?? "compressed";
var fullPath = Path.Combine(app.Environment.ContentRootPath, outputDir);
if (!Directory.Exists(fullPath))
{
    Directory.CreateDirectory(fullPath);
}


app.UseSwagger();
app.UseSwaggerUI();

// Static files to serve compressed results
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(fullPath),
    RequestPath = $"/{outputDir}"
});

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
