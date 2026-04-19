using Microsoft.AspNetCore.Http;

namespace StoreMaster.Core.Interfaces;

public interface IFileService
{
    Task<(bool Success, string Message, string? FileName)> SaveImageAsync(
        IFormFile file, string folder);
    void DeleteImage(string? fileName, string folder);
}