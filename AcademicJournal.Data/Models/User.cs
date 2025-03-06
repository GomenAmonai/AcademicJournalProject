namespace AcademicJournal.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public DateTime? LastLoginDate { get; set; }
        
        // Навигационные свойства
        public Student? Student { get; set; }
        public Teacher? Teacher { get; set; }
    }
}