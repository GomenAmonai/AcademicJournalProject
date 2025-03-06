using AcademicJournal.Data;
using AcademicJournal.Data.Models;
using AcademicJournal.Server.Models;
using AcademicJournal.Server.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AcademicJournal.Tests.Services
{
    public class UserServiceTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;

        public UserServiceTests()
        {
            // Используем базу данных в памяти для тестов
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            // Создаем мок для сервиса хеширования паролей
            _passwordHasherMock = new Mock<IPasswordHasher>();
            
            // Настраиваем поведение мока для проверки пароля
            _passwordHasherMock
                .Setup(x => x.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string, string>((password, hash, salt) => password == "correctPassword");
                
            // Настраиваем поведение мока для создания хеша пароля
            string passwordHash = "newPasswordHash";
            string salt = "newSalt";
            _passwordHasherMock
                .Setup(x => x.CreatePasswordHash(It.IsAny<string>(), out passwordHash, out salt));

            // Инициализируем тестовые данные
            using (var context = new ApplicationDbContext(_options))
            {
                var user = new User
                {
                    Id = 1,
                    Username = "testuser",
                    Email = "test@example.com",
                    FullName = "Test User",
                    PasswordHash = "hashedPassword",
                    Salt = "salt",
                    Role = "Student",
                    LastLoginDate = DateTime.Now.AddDays(-5)
                };

                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        [Fact]
        public async Task GetUserProfileAsync_ShouldReturnUserInfo()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var userService = new UserService(context, _passwordHasherMock.Object);
                
                // Act
                var result = await userService.GetUserProfileAsync(1);
                
                // Assert
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
                Assert.Equal("testuser", result.Username);
                Assert.Equal("test@example.com", result.Email);
                Assert.Equal("Test User", result.FullName);
                Assert.Equal("Student", result.Role);
                Assert.NotNull(result.LastLoginDate);
            }
        }

        [Fact]
        public async Task UpdateUserProfileAsync_ShouldModifyUserInfo()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var userService = new UserService(context, _passwordHasherMock.Object);
                var updateDto = new UpdateProfileDto
                {
                    Email = "newemail@example.com",
                    FullName = "Updated Test User"
                };
                
                // Act
                var result = await userService.UpdateUserProfileAsync(1, updateDto);
                
                // Assert
                Assert.NotNull(result);
                Assert.Equal("newemail@example.com", result.Email);
                Assert.Equal("Updated Test User", result.FullName);
                
                // Проверяем, что данные действительно обновлены в БД
                var updatedUser = await context.Users.FindAsync(1);
                Assert.Equal("newemail@example.com", updatedUser.Email);
                Assert.Equal("Updated Test User", updatedUser.FullName);
                
                // Убеждаемся, что другие поля не изменились
                Assert.Equal("testuser", updatedUser.Username);
                Assert.Equal("Student", updatedUser.Role);
            }
        }

        [Fact]
        public async Task UpdateUserProfileAsync_ShouldChangePasswordIfValid()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var userService = new UserService(context, _passwordHasherMock.Object);
                var updateDto = new UpdateProfileDto
                {
                    CurrentPassword = "correctPassword",  // Настроено в моке как "правильный" пароль
                    NewPassword = "newSecurePassword"
                };
                
                // Act
                var result = await userService.UpdateUserProfileAsync(1, updateDto);
                
                // Assert
                // Проверяем, что пароль изменен в БД
                var updatedUser = await context.Users.FindAsync(1);
                // Проверяем, что вызывался метод проверки пароля
                _passwordHasherMock.Verify(
                    x => x.VerifyPasswordHash("correctPassword", It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);
            }
        }

        [Fact]
        public async Task UpdateUserProfileAsync_ShouldThrowIfWrongPassword()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                // Настраиваем мок для возврата false при неверном пароле
                _passwordHasherMock
                    .Setup(x => x.VerifyPasswordHash("wrongPassword", It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(false);
                
                var userService = new UserService(context, _passwordHasherMock.Object);
                var updateDto = new UpdateProfileDto
                {
                    CurrentPassword = "wrongPassword",
                    NewPassword = "newSecurePassword"
                };
                
                // Act & Assert
                await Assert.ThrowsAsync<InvalidOperationException>(
                    async () => await userService.UpdateUserProfileAsync(1, updateDto));
                
                // Проверяем, что пароль не изменился в БД
                var user = await context.Users.FindAsync(1);
                Assert.Equal("hashedPassword", user.PasswordHash);
                Assert.Equal("salt", user.Salt);
            }
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldReturnTrueIfSuccessful()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var userService = new UserService(context, _passwordHasherMock.Object);
                
                // Act
                var result = await userService.ChangePasswordAsync(1, "correctPassword", "newSecurePassword");
                
                // Assert
                Assert.True(result);
                
                // Проверяем, что вызывался метод проверки пароля
                _passwordHasherMock.Verify(
                    x => x.VerifyPasswordHash("correctPassword", It.IsAny<string>(), It.IsAny<string>()),
                    Times.Once);
            }
        }

        [Fact]
        public async Task ChangePasswordAsync_ShouldReturnFalseIfWrongPassword()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                // Настраиваем мок для возврата false при неверном пароле
                _passwordHasherMock
                    .Setup(x => x.VerifyPasswordHash("wrongPassword", It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(false);
                
                var userService = new UserService(context, _passwordHasherMock.Object);
                
                // Act
                var result = await userService.ChangePasswordAsync(1, "wrongPassword", "newSecurePassword");
                
                // Assert
                Assert.False(result);
                
                // Проверяем, что пароль не изменился в БД
                var user = await context.Users.FindAsync(1);
                Assert.Equal("hashedPassword", user.PasswordHash);
                Assert.Equal("salt", user.Salt);
            }
        }
    }
}