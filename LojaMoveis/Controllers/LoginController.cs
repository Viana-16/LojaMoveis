using LojaMoveis.DTO;
using LojaMoveis.Services;
using Microsoft.AspNetCore.Mvc;

namespace LojaMoveis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AdminService _adminService;
        private readonly ClienteService _clienteService;

        public LoginController(AdminService adminService, ClienteService clienteService)
        {
            _adminService = adminService;
            _clienteService = clienteService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            // Tenta autenticar como admin
            var admin = await _adminService.GetByEmailAsync(login.Email);
            if (admin != null && BCrypt.Net.BCrypt.Verify(login.Senha, admin.Senha))
            {
                return Ok(new { tipo = "admin", mensagem = "Login de admin realizado com sucesso." });
            }

            // Tenta autenticar como cliente
            var cliente = await _clienteService.GetByEmailAsync(login.Email);
            if (cliente != null && BCrypt.Net.BCrypt.Verify(login.Senha, cliente.Senha))
            {
                return Ok(new { tipo = "cliente", mensagem = "Login de cliente realizado com sucesso." });
            }

            return Unauthorized("Credenciais inválidas.");
        }
    }
}
