using Microsoft.AspNetCore.Mvc;
using AcademicJournal.Server.Services;
using System.Threading.Tasks;
using AcademicJournal.Data.Models;
using AcademicJournal.Server.Models;

namespace AcademicJournal.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var result = await _authService.AuthenticateAsync(model.Username, model.Password);
            
            if (result.Success)
            {
                return Ok(new { Token = result.Token });
            }
            
            return Unauthorized(result.Message);
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _authService.RegisterAsync(model);
            
            if (result.Success)
            {
                return Ok(new { Message = "Пользователь успешно зарегистрирован" });
            }
            
            return BadRequest(result.Message);
        }
    }
}