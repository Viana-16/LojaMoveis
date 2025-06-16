using LojaMoveis.DTO;
using LojaMoveis.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            var admin = await _adminService.GetByEmailAsync(login.Email);
            var cliente = await _clienteService.GetByEmailAsync(login.Email);
            string tipo = "";
            string id = "";
            string email = "";
            string cpf = "";
            string telefone = "";

            if (admin != null && BCrypt.Net.BCrypt.Verify(login.Senha, admin.Senha))
            {
                tipo = "admin";
                id = admin.Id;
                email = admin.Email;
            }
            else if (cliente != null && BCrypt.Net.BCrypt.Verify(login.Senha, cliente.Senha))
            {
                tipo = "cliente";
                id = cliente.Id;
                email = cliente.Email;
                cpf = cliente.Cpf;
                telefone = cliente.Telefone;
            }
            else
            {
                return Unauthorized("Credenciais inválidas.");
            }

            // Gerar token JWT
            var token = TokenService.GenerateToken(id, tipo, email);

            // Definir cookie seguro
            Response.Cookies.Append("jwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // true em produção com HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(2)
            });

            return Ok(new { tipo, email });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok(new { mensagem = "Logout realizado com sucesso." });
        }


        [HttpGet("perfil")]
        [Authorize]
        public async Task<IActionResult> Perfil()

        {
            var token = Request.Cookies["jwt"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { mensagem = "Usuário não autenticado." });

            var principal = TokenService.ValidarToken(token);
            if (principal == null)
                return Unauthorized(new { mensagem = "Token inválido ou expirado." });

            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            var tipo = principal.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new { email, tipo });
        }
    }
    
}

