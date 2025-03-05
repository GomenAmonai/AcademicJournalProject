using AcademicJournal.Data;
using AcademicJournal.Data.Models;
using AcademicJournal.Server.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AcademicJournal.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtGenerator _jwtGenerator;
        
        public AuthService(
            ApplicationDbContext context, 
            IPasswordHasher passwordHasher,
            IJwtGenerator jwtGenerator)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtGenerator = jwtGenerator;
        }
        
        public async Task<AuthResult> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
                
            if (user == null)
            {
                return new AuthResult { Success = false, Message = "Пользователь не найден" };
            }
            
            if (!_passwordHasher.VerifyPasswordHash(password, user.PasswordHash, user.Salt))
            {
                return new AuthResult { Success = false, Message = "Неверный пароль" };
            }
            
            var token = _jwtGenerator.GenerateToken(user);
            
            user.LastLoginDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            
            return new AuthResult { Success = true, Token = token };
        }
        
        public async Task<AuthResult> RegisterAsync(RegisterDto model)
        {
            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
            {
                return new AuthResult { Success = false, Message = "Имя пользователя уже занято" };
            }
            
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                return new AuthResult { Success = false, Message = "Email уже зарегистрирован" };
            }
            
            _passwordHasher.CreatePasswordHash(model.Password, out string passwordHash, out string salt);
            
            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = passwordHash,
                Salt = salt,
                FullName = model.FullName,
                Role = model.Role,
                LastLoginDate = null
            };
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return new AuthResult { Success = true };
        }
    }
}