using LojaMoveis.Models;
using LojaMoveis.Services;
using Microsoft.AspNetCore.Mvc;

namespace LojaMoveis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Admin adminLogin)
        {
            var admin = await _adminService.LoginAsync(adminLogin.Email, adminLogin.Senha);
            if (admin == null)
                return Unauthorized("Credenciais inválidas");

            return Ok(new { message = "Login realizado com sucesso" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Admin admin)
        {
            await _adminService.CadastrarAdminAsync(admin);
            return Ok(new { message = "Admin cadastrado" });
        }

    }
}
