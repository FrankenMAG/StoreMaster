using StoreMaster.Core.Interfaces;

namespace StoreMaster.Web.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _environment;
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
    private const long MaxFileSize = 2 * 1024 * 1024; // 2MB

    public FileService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<(bool Success, string Message, string? FileName)> SaveImageAsync(
        IFormFile file, string folder)
    {
        // Validar tamaño
        if (file.Length > MaxFileSize)
            return (false, "La imagen no puede superar 2MB.", null);

        // Validar extensión
        var extension = Path.GetExtension(file.FileName).ToLower();
        if (!_allowedExtensions.Contains(extension))
            return (false, "Solo se permiten imágenes JPG, PNG o WEBP.", null);

        // Crear carpeta si no existe
        var folderPath = Path.Combine(_environment.WebRootPath, "images", folder);
        Directory.CreateDirectory(folderPath);

        // Generar nombre único para evitar colisiones
        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(folderPath, fileName);

        // Guardar el archivo
        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return (true, "Imagen guardada correctamente.", fileName);
    }

    public void DeleteImage(string? fileName, string folder)
    {
        if (string.IsNullOrEmpty(fileName))
            return;

        var filePath = Path.Combine(_environment.WebRootPath, "images", folder, fileName);
        if (File.Exists(filePath))
            File.Delete(filePath);
    }
}