/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

namespace GLMS.Web.Services
{
    public class FileService : IFileService
    {
        private static readonly HashSet<string> AllowedExtensions =
            new(StringComparer.OrdinalIgnoreCase) { ".pdf" };

        public void ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new InvalidOperationException("No file was uploaded.");

            var extension = Path.GetExtension(file.FileName);
            if (!AllowedExtensions.Contains(extension))
                throw new InvalidOperationException(
                    $"Invalid file type '{extension}'. Only .pdf files are allowed.");
        }

        public async Task<string> SaveFileAsync(IFormFile file, string uploadsPath)
        {
            Directory.CreateDirectory(uploadsPath);
            var uniqueName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName).ToLowerInvariant()}";
            var fullPath = Path.Combine(uploadsPath, uniqueName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return uniqueName;
        }
    }
}
