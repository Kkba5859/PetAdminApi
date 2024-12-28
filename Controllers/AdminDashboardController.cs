using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PetAdminApi.Data;
using PetAdminApi.Hubs;

namespace PetAdminApi.Controllers
{
    [Route("api/AdminDashboard")]
    [ApiController]
    public class AdminDashboardController : ControllerBase
    {
        private readonly UserDbContext _userContext;
        private readonly AdminDbContext _adminContext;
        private readonly IHubContext<NotificationHub> _hubContext;

        public AdminDashboardController(UserDbContext userContext, AdminDbContext adminContext, IHubContext<NotificationHub> hubContext)
        {
            _userContext = userContext;
            _adminContext = adminContext;
            _hubContext = hubContext;
        }

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

            // Отправка уведомления через SignalR
            await _hubContext.Clients.All.SendAsync("UserDeleted", username);

            return Ok(new { message = "Пользователь удален." });
        }

        [HttpDelete("deleteAdmin/{username}")]
        public async Task<IActionResult> DeleteAdmin(string username)
        {
            var admin = await _adminContext.Admins
                .Where(a => a.Username == username)
                .FirstOrDefaultAsync();

            if (admin == null)
            {
                return NotFound(new { message = "Администратор не найден." });
            }

            _adminContext.Admins.Remove(admin);
            await _adminContext.SaveChangesAsync();

            // Отправка уведомления через SignalR
            await _hubContext.Clients.All.SendAsync("AdminDeleted", username);

            return Ok(new { message = "Администратор удален." });
        }

        [HttpGet("getUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userContext.Users
                .Take(100)
                .Select(u => new { u.Id, u.Username })
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("getAdmins")]
        public async Task<IActionResult> GetAdmins()
        {
            var admins = await _adminContext.Admins
                .Take(100)
                .Select(a => new { a.Id, a.Username })
                .ToListAsync();

            return Ok(admins);
        }
    }
}
