namespace AcademicJournal.Server.Models
{
    public class UpdateUserDto
    {
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Password { get; set; } // Опционально
    }
}