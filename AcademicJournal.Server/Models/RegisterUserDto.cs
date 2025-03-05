namespace AcademicJournal.Server.Models
{
    // Можно использовать RegisterDto, но создадим отдельный класс для соответствия интерфейсу
    public class RegisterUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}