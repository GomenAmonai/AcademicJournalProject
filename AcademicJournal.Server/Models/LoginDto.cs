using System.ComponentModel.DataAnnotations;

namespace AcademicJournal.Data.Models;

public class LoginDto
{
    [Required(ErrorMessage = "Имя пользователя обязательно")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "Пароль обязателен")]
    public string Password { get; set; }
}