# 🖼️ ImageCompressionService

A lightweight and configurable image compression service built with .NET 8. It supports JPG/PNG uploads, adjustable quality, optional PNG → JPEG conversion, resizing with aspect ratio preservation, and file storage with public links — ideal for real-world use and portfolio projects.

---

## 📁 Project Structure

- `ImageCompress.Api` – Web API layer for receiving image requests  
- `ImageCompress.Application` – DTOs, validation, and mapping logic  
- `ImageCompress.Data` – Infrastructure implementations (ImageSharp compressor, storage services)  
- `ImageCompress.Domain` – Core models, entities, and interfaces  

---

## 🛠 Technologies Used

- **.NET 8** – Core framework for building the service  
- **ASP.NET Core Web API** – For exposing the compression endpoint  
- **ImageSharp** – For image processing and compression  
- **FluentValidation** – For validating incoming requests  
- **Dependency Injection (DI)** – For clean and testable architecture  
- **Options Pattern** – For reading compression and storage settings from configuration  
- **IFormFile** – For handling file uploads via API  
- **Swagger / Swashbuckle** – For API documentation and testing  
- **Clean Architecture** – For separation of concerns and maintainability  

---

## ⚙️ Configuration Example (appsettings.json)

Before running the project, make sure to update your `appsettings.json` file with your own compression and storage settings:

```json
{
  "Compression": {
    "DefaultQuality": 70,
    "MaxInputBytes": 5242880,
    "Resize": {
      "MaxWidth": 1920,
      "MaxHeight": 1080,
      "Enabled": true
    }
  },
  "Storage": {
    "Mode": "FileSystem",
    "FileSystem": {
      "OutputDir": "compressed"
    }
  }
}
```
## Example Response (when returnAsLink = true):
```json
{
  "url": "/compressed/sample_compressed.jpg",
  "inputBytes": 1048576,
  "outputBytes": 345678,
  "ratio": 0.33,
  "durationMs": 120,
  "format": "JPEG"
}
```
## Example Response (when returnAsLink = false): 
The API returns the compressed file directly as a download stream.

---

## 📦 Example Workflow

1. **Start the API**  
   Run the project with `dotnet run` from the `ImageCompress.Api` folder.  
   Swagger UI will be available at: `https://localhost:5001/swagger`

2. **Upload an Image**  
   Use Swagger or Postman to send a `multipart/form-data` request to `/api/compress`.

3. **Receive the Result**  
   - If `returnAsLink = true`, the API responds with a JSON object containing a public URL to the compressed file.  
   - If `returnAsLink = false`, the API streams the compressed file directly as a download.

---

## 🚀 Future Improvements

- Add support for more formats (e.g., WebP, GIF).  
- Implement caching for repeated requests.  
- Add authentication/authorization for secured endpoints.  
- Provide Dockerfile for containerized deployment.  

---

## 📜 License

This project is licensed under the MIT License.  
You are free to use, modify, and distribute it for personal or commercial purposes.
