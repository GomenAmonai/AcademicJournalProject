using System.ComponentModel.DataAnnotations;

namespace AcademicJournal.Data.Models;

public class RegisterDto
{
    [Required(ErrorMessage = "Имя пользователя обязательно")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Некорректный формат Email")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Пароль обязателен")]
    [MinLength(6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Полное имя обязательно")]
    public string FullName { get; set; }
    
    [Required(ErrorMessage = "Роль обязательна")]
    public string Role { get; set; }
}