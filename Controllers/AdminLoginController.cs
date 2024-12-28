using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using PetAdminApi.Data;
using PetAdminApi.Models;

namespace PetAdminApi.Controllers
{
    [Route("api/AdminLogin")]
    [ApiController]
    public class AdminLoginController : ControllerBase
    {
        private readonly AdminDbContext _adminContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AdminLoginController(AdminDbContext adminContext, IConfiguration configuration, IMapper mapper)
        {
            _adminContext = adminContext;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult Login([FromBody] AdminDto request)
        {
            var admin = _adminContext.Admins.FirstOrDefault(a => a.Username == request.Username);
            if (admin == null || !BCrypt.Net.BCrypt.Verify(request.Password, admin.PasswordHash))
            {
                return Unauthorized("Неверные учетные данные.");
            }

            return Ok(new { message = "Login successful" });
        }
    }
}
