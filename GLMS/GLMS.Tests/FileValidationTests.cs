/*
 * Pro C# 10 with .NET 6 Foundational Principles and Practices in Programming
 * Eleventh Edition
 * Andrew Troelsen, Philip Japikse
 * Apress, 2022
 */

using GLMS.Web.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace GLMS.Tests
{
    public class FileValidationTests
    {
        private readonly FileService _fileService = new FileService();

        private static IFormFile MockFile(string fileName, long size = 1024)
        {
            var mock = new Mock<IFormFile>();
            mock.Setup(f => f.FileName).Returns(fileName);
            mock.Setup(f => f.Length).Returns(size);
            return mock.Object;
        }

        [Fact]
        public void ValidateFile_PdfFile_DoesNotThrow()
        {
            var file = MockFile("agreement.pdf");
            var ex = Record.Exception(() => _fileService.ValidateFile(file));
            Assert.Null(ex);
        }

        [Fact]
        public void ValidateFile_ExeFile_ThrowsInvalidOperationException()
        {
            var file = MockFile("malware.exe");
            Assert.Throws<InvalidOperationException>(() => _fileService.ValidateFile(file));
        }

        [Fact]
        public void ValidateFile_DocxFile_ThrowsInvalidOperationException()
        {
            var file = MockFile("contract.docx");
            Assert.Throws<InvalidOperationException>(() => _fileService.ValidateFile(file));
        }

        [Fact]
        public void ValidateFile_JpgFile_ThrowsInvalidOperationException()
        {
            var file = MockFile("scan.jpg");
            Assert.Throws<InvalidOperationException>(() => _fileService.ValidateFile(file));
        }

        [Fact]
        public void ValidateFile_NullFile_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => _fileService.ValidateFile(null!));
        }

        [Fact]
        public void ValidateFile_EmptyFile_ThrowsInvalidOperationException()
        {
            var file = MockFile("empty.pdf", size: 0);
            Assert.Throws<InvalidOperationException>(() => _fileService.ValidateFile(file));
        }

        [Fact]
        public void ValidateFile_ExeErrorMessage_ContainsExtension()
        {
            var file = MockFile("virus.exe");
            var ex = Assert.Throws<InvalidOperationException>(() => _fileService.ValidateFile(file));
            Assert.Contains(".exe", ex.Message);
        }
    }
}
