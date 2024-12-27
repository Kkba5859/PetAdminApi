using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetAdminApi.Data;
using PetAdminApi.Models;
using System.Threading.Tasks;

namespace PetAdminApi.Controllers
{
    [Route("api/Admin/dashboard")]
    [ApiController]
    public class AdminDashboardController : ControllerBase
    {
        private readonly UserDbContext _userContext;  // Используем UserDbContext

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
                return NotFound("Пользователь не найден.");
            }

            _userContext.Users.Remove(user);
            await _userContext.SaveChangesAsync();

            return Ok("Пользователь удален.");
        }

        // Метод для получения списка всех пользователей
        [HttpGet("getUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userContext.Users.ToListAsync();
            return Ok(users);
        }
    }
}
