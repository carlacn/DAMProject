using DAMProject.Shared.Request;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;

namespace DAMProject.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController(IWebHostEnvironment environment) : ControllerBase
    {
        private readonly IWebHostEnvironment _environment = environment;

        [HttpPost]
        [Route("upload-image")]
        public async Task<IActionResult> UploadImage([FromForm] FileUploadRequest request)
        {
            try
            {
                if (request.File == null || request.File.Length == 0)
                    return BadRequest("No se ha recibido ningún archivo.");

                var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "Images");

                if (!Directory.Exists(imagesPath))
                {
                    Directory.CreateDirectory(imagesPath);
                }

                var filePath = Path.Combine(imagesPath, request.File.FileName);

                const int targetHeight = 550;


                //using (var stream = new FileStream(filePath, FileMode.Create))
                //{
                //    await request.File.CopyToAsync(stream);
                //}
                using (var image = await Image.LoadAsync(request.File.OpenReadStream())) 
                {
                    var aspectRatio = (double)image.Width / image.Height;
                    var targetWidth = (int)(targetHeight * aspectRatio);
                    image.Mutate(x => x.Resize(targetWidth, targetHeight));

                    using (var memoryStream = new MemoryStream()) 
                    {
                        var encoder = GetEncoder(filePath);
                        await image.SaveAsync(memoryStream, encoder);
                        await System.IO.File.WriteAllBytesAsync(filePath, memoryStream.ToArray());
                    }
                }

                return Ok(new { Message = "Imagen subida con éxito.", Path = filePath });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al guardar el archivo: {ex.Message}");
            }
        }

        [HttpGet("{imageName}")]
        public IActionResult GetImage(string imageName)
        {
            var imagesFolder = Path.Combine(_environment.ContentRootPath, "Images");
            var filePath = Path.Combine(imagesFolder, imageName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var contentType = GetContentType(filePath);
            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, contentType, imageName);
        }

        private string GetContentType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".svg" => "image/svg+xml",
                _ => "application/octet-stream",
            };
        }

        private static IImageEncoder GetEncoder(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();

            return extension switch
            {
                ".jpg" or ".jpeg" => new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder(),
                ".png" => new SixLabors.ImageSharp.Formats.Png.PngEncoder(),
                ".bmp" => new SixLabors.ImageSharp.Formats.Bmp.BmpEncoder(),
                ".gif" => new SixLabors.ImageSharp.Formats.Gif.GifEncoder(),
                _ => throw new NotSupportedException($"Formato de archivo no soportado: {extension}")
            };
        }
    }
}
