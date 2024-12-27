using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetAdminApi.Data;
using PetAdminApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PetAdminApi.Controllers
{
    [Route("api/Admin/login")]
    [ApiController]
    public class AdminLoginController : ControllerBase
    {
        private readonly AdminDbContext _adminContext;  // Используем AdminDbContext
        private readonly IConfiguration _configuration;

        public AdminLoginController(AdminDbContext adminContext, IConfiguration configuration)
        {
            _adminContext = adminContext;
            _configuration = configuration;
        }

        // Метод для входа администратора
        [HttpPost]
        public IActionResult Login([FromBody] AdminDto request)
        {
            // Поиск администратора по имени
            var admin = _adminContext.Admins.FirstOrDefault(a => a.Username == request.Username);
            if (admin == null || !BCrypt.Net.BCrypt.Verify(request.Password, admin.PasswordHash))
            {
                return Unauthorized("Неверные учетные данные.");
            }

            // Создание JWT токена
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, admin.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}
