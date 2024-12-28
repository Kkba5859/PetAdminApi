using Microsoft.AspNetCore.Mvc;
using PetAdminApi.Data;
using PetAdminApi.Models;
using System.Linq;

namespace PetAdminApi.Controllers
{
    [Route("api/AdminLogin")]
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

            // Успешный вход
            return Ok(new { message = "Login successful" });
        }
    }
}
