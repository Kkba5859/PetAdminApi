using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetAdminApi.Data;
using PetAdminApi.Models;


namespace PetAdminApi.Controllers
{
    [Route("api/AdminRegister")]
    [ApiController]
    public class AdminRegisterController : ControllerBase
    {
        private readonly AdminDbContext _adminContext;  // Используем AdminDbContext

        public AdminRegisterController(AdminDbContext adminContext)
        {
            _adminContext = adminContext;
        }

        // Метод регистрации администратора
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] AdminDto request)
        {
            if (request == null)
            {
                return BadRequest("Данные не были отправлены.");
            }

            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Имя пользователя или пароль не могут быть пустыми.");
            }

            // Проверка существования администратора
            var existingAdmin = await _adminContext.Admins
                .Where(a => a.Username == request.Username)
                .FirstOrDefaultAsync();

            if (existingAdmin != null)
            {
                return BadRequest("Администратор с таким именем уже существует.");
            }

            // Создание нового администратора
            var admin = new Admin
            {
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            await _adminContext.Admins.AddAsync(admin);
            await _adminContext.SaveChangesAsync();

            return Ok("Администратор успешно зарегистрирован.");
        }

    }
}
