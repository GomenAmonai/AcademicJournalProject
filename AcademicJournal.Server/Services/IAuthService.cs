using System.Threading.Tasks;
using AcademicJournal.Data.Models;

namespace AcademicJournal.Server.Services
{
    public interface IAuthService
    {
        Task<AuthResult> AuthenticateAsync(string username, string password);
        Task<AuthResult> RegisterAsync(RegisterDto model);
    }
    
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}