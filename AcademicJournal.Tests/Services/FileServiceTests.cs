using AcademicJournal.Data;
using AcademicJournal.Data.Models;
using AcademicJournal.Server.Models;
using AcademicJournal.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AcademicJournal.Tests.Services
{
    public class FileServiceTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public FileServiceTests()
        {
            // Используем базу данных в памяти для тестов
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            // Инициализируем тестовые данные
            using (var context = new ApplicationDbContext(_options))
            {
                // Создаем тестового пользователя
                var user = new User
                {
                    Id = 1,
                    Username = "testuser",
                    FullName = "Test User"
                };

                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        private IFormFile CreateTestFile(string fileName, string content)
        {
            // Создаем мок для IFormFile
            var fileMock = new Mock<IFormFile>();
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.ContentType).Returns("text/plain");
            
            // Настраиваем метод CopyToAsync для правильной работы
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns((Stream stream, CancellationToken token) => {
                    ms.Position = 0;
                    return ms.CopyToAsync(stream);
                });
            
            return fileMock.Object;
        }

        [Fact]
        public async Task UploadFileAsync_ShouldSaveFileAndReturnInfo()
        {
            // Arrange
            var testFile = CreateTestFile("test.txt", "This is a test file content");
            
            using (var context = new ApplicationDbContext(_options))
            {
                var fileService = new FileService(context);
                
                // Act
                var result = await fileService.UploadFileAsync(testFile, 1);
                
                // Assert
                Assert.NotNull(result);
                Assert.Equal("test.txt", result.OriginalFileName);
                Assert.Equal("text/plain", result.ContentType);
                Assert.Equal(testFile.Length, result.Size);
                Assert.Equal(1, result.UploadedById);
                
                // Проверяем, что файл сохранен в БД
                var savedFile = await context.Files.FindAsync(result.Id);
                Assert.NotNull(savedFile);
                Assert.Equal("test.txt", savedFile.OriginalFileName);
                Assert.Equal(testFile.Length, savedFile.Size);
                
                // Проверяем содержимое файла
                var fileContent = Encoding.UTF8.GetString(savedFile.Data);
                Assert.Equal("This is a test file content", fileContent);
            }
        }

        [Fact]
        public async Task GetFileInfoAsync_ShouldReturnFileMetadata()
        {
            // Arrange
            int fileId;
            using (var context = new ApplicationDbContext(_options))
            {
                var file = new FileEntity
                {
                    FileName = Guid.NewGuid().ToString(),
                    OriginalFileName = "metadata_test.txt",
                    ContentType = "text/plain",
                    Data = Encoding.UTF8.GetBytes("Content for metadata test"),
                    Size = 24,
                    UploadDate = DateTime.Now,
                    UploadedById = 1
                    // Добавьте другие обязательные поля, если они есть в вашей модели
                };
        
                context.Files.Add(file);
                await context.SaveChangesAsync();
                fileId = file.Id;
        
                // Проверяем, что файл действительно добавлен
                var addedFile = await context.Files.FindAsync(fileId);
                Assert.NotNull(addedFile); // Убедимся, что файл добавлен
            }
    
            using (var context = new ApplicationDbContext(_options))
            {
                var fileService = new FileService(context);
        
                // Для отладки: проверим, есть ли файл в базе
                var dbFile = await context.Files.FindAsync(fileId);
                Assert.NotNull(dbFile); // Эта проверка поможет определить, сохраняется ли файл между контекстами
        
                // Act
                var result = await fileService.GetFileInfoAsync(fileId);
        
                // Assert
                Assert.NotNull(result);
                Assert.Equal("metadata_test.txt", result.OriginalFileName);
                Assert.Equal("text/plain", result.ContentType);
                Assert.Equal(24, result.Size);
                Assert.Equal(1, result.UploadedById);
            }
        
            
            using (var context = new ApplicationDbContext(_options))
            {
                var fileService = new FileService(context);
                
                // Act
                var result = await fileService.GetFileInfoAsync(fileId);
                
                // Assert
                Assert.NotNull(result);
                Assert.Equal("metadata_test.txt", result.OriginalFileName);
                Assert.Equal("text/plain", result.ContentType);
                Assert.Equal(24, result.Size);
                Assert.Equal(1, result.UploadedById);
            }
        }

        [Fact]
        public async Task DownloadFileAsync_ShouldReturnFileContent()
        {
            // Arrange
            int fileId;
            byte[] expectedContent = Encoding.UTF8.GetBytes("Content for download test");
            
            using (var context = new ApplicationDbContext(_options))
            {
                var file = new FileEntity
                {
                    FileName = Guid.NewGuid().ToString(),
                    OriginalFileName = "download_test.txt",
                    ContentType = "text/plain",
                    Data = expectedContent,
                    Size = expectedContent.Length,
                    UploadDate = DateTime.Now,
                    UploadedById = 1
                };
                
                context.Files.Add(file);
                await context.SaveChangesAsync();
                fileId = file.Id;
            }
            
            using (var context = new ApplicationDbContext(_options))
            {
                var fileService = new FileService(context);
                
                // Act
                var result = await fileService.DownloadFileAsync(fileId);
                
                // Assert
                Assert.NotNull(result);
                Assert.Equal(expectedContent.Length, result.Length);
                Assert.Equal(expectedContent, result);  // Сравниваем байты
            }
        }

        [Fact]
        public async Task DeleteFileAsync_ShouldRemoveFileFromDb()
        {
            // Arrange
            int fileId;
            using (var context = new ApplicationDbContext(_options))
            {
                var file = new FileEntity
                {
                    FileName = Guid.NewGuid().ToString(),
                    OriginalFileName = "delete_test.txt",
                    ContentType = "text/plain",
                    Data = Encoding.UTF8.GetBytes("To be deleted"),
                    Size = 12,
                    UploadDate = DateTime.Now,
                    UploadedById = 1
                };
                
                context.Files.Add(file);
                await context.SaveChangesAsync();
                fileId = file.Id;
            }
            
            using (var context = new ApplicationDbContext(_options))
            {
                var fileService = new FileService(context);
                
                // Act
                await fileService.DeleteFileAsync(fileId);
                
                // Assert
                var deletedFile = await context.Files.FindAsync(fileId);
                Assert.Null(deletedFile);  // Файл должен быть удален
            }
        }

        [Fact]
        public async Task GetUserFilesAsync_ShouldReturnPaginatedResults()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                // Добавляем несколько файлов для тестового пользователя
                for (int i = 0; i < 15; i++)
                {
                    context.Files.Add(new FileEntity
                    {
                        FileName = Guid.NewGuid().ToString(),
                        OriginalFileName = $"user_file_{i}.txt",
                        ContentType = "text/plain",
                        Data = Encoding.UTF8.GetBytes($"Content for file {i}"),
                        Size = 15 + i,
                        UploadDate = DateTime.Now.AddHours(-i),
                        UploadedById = 1
                    });
                }
                await context.SaveChangesAsync();
            }
            
            using (var context = new ApplicationDbContext(_options))
            {
                var fileService = new FileService(context);
                
                // Act
                var result = await fileService.GetUserFilesAsync(1, 1, 10);
                
                // Assert
                Assert.NotNull(result);
                Assert.Equal(15, result.TotalCount);  // Всего 15 файлов
                Assert.Equal(10, result.Items.Count); // Но на первой странице только 10
                Assert.Equal(1, result.Page);
                Assert.Equal(10, result.PageSize);
                
                // Проверяем, что файлы отсортированы по дате загрузки (сначала новые)
                Assert.Equal("user_file_0.txt", result.Items[0].OriginalFileName);
                Assert.Equal("user_file_9.txt", result.Items[9].OriginalFileName);
            }
        }
    }
}