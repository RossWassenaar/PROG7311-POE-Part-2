/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

namespace GLMS.Web.Services
{
    public interface IFileService
    {
        void ValidateFile(IFormFile file);
        Task<string> SaveFileAsync(IFormFile file, string uploadsPath);
    }
}
