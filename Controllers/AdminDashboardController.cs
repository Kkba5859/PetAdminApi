using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetAdminApi.Data;

namespace PetAdminApi.Controllers
{
    [Route("api/AdminDashboard")]
    [ApiController]
    public class AdminDashboardController : ControllerBase
    {
        private readonly UserDbContext _userContext;

        public AdminDashboardController(UserDbContext userContext)
        {
            _userContext = userContext;
        }

        // Метод для удаления пользователя
        [HttpDelete("deleteUser/{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var user = await _userContext.Users
                .Where(u => u.Username == username)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { message = "Пользователь не найден." });
            }

            _userContext.Users.Remove(user);
            await _userContext.SaveChangesAsync();

            return Ok(new { message = "Пользователь удален." });
        }

        // Метод для получения списка всех пользователей
        [HttpGet("getUsers")]
        public async Task<IActionResult> GetUsers()
        {
            // Получаем список пользователей с только нужными полями
            var users = await _userContext.Users
                .Take(100)
                .Select(u => new { u.Id, u.Username })
                .ToListAsync();

            return Ok(users); // Этот результат будет сериализован в JSON
        }
    }
}
