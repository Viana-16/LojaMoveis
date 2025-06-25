//using LojaMoveis.DTO;
//using LojaMoveis.Models;
//using LojaMoveis.Services;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace LojaMoveis.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class LoginController : ControllerBase
//    {
//        private readonly AdminService _adminService;
//        private readonly ClienteService _clienteService;
//        private readonly EmailService _emailService;
//        private readonly ResetTokenService _resetTokenService;
//        private readonly TokenService _tokenService;

//        public LoginController(AdminService adminService, ClienteService clienteService, EmailService emailService, ResetTokenService resetTokenService, TokenService tokenService)
//        {
//            _adminService = adminService;
//            _clienteService = clienteService;
//            _emailService = emailService;
//            _resetTokenService = resetTokenService;
//            _tokenService = tokenService;
//        }

//        [HttpPost]
//        public async Task<IActionResult> Login([FromBody] LoginDto login)
//        {
//            var admin = await _adminService.GetByEmailAsync(login.Email);
//            var cliente = await _clienteService.GetByEmailAsync(login.Email);

//            string tipo = "";
//            string id = "";
//            string email = "";
//            string cpf = "";
//            string telefone = "";

//            if (admin != null && BCrypt.Net.BCrypt.Verify(login.Senha, admin.Senha))
//            {
//                tipo = "admin";
//                id = admin.Id;
//                email = admin.Email;
//            }
//            else if (cliente != null && BCrypt.Net.BCrypt.Verify(login.Senha, cliente.Senha))
//            {
//                tipo = "cliente";
//                id = cliente.Id;
//                email = cliente.Email;
//                cpf = cliente.Cpf;
//                telefone = cliente.Telefone;
//            }
//            else
//            {
//                return Unauthorized("Credenciais inválidas.");
//            }

//            var token = TokenService.GenerateToken(id, tipo, email);

//            Response.Cookies.Append("jwtToken", token, new CookieOptions
//            {
//                HttpOnly = true,
//                Secure = true,
//                SameSite = SameSiteMode.Strict,
//                Expires = DateTimeOffset.UtcNow.AddHours(2)
//            });

//            return Ok(new { tipo, email });
//        }

//        [HttpPost("logout")]
//        public IActionResult Logout()
//        {
//            Response.Cookies.Delete("jwt");
//            return Ok(new { mensagem = "Logout realizado com sucesso." });
//        }

//        [HttpGet("perfil")]
//        [Authorize]
//        public async Task<IActionResult> Perfil()
//        {
//            var token = Request.Cookies["jwt"];

//            if (string.IsNullOrEmpty(token))
//                return Unauthorized(new { mensagem = "Usuário não autenticado." });

//            var principal = TokenService.ValidarToken(token);
//            if (principal == null)
//                return Unauthorized(new { mensagem = "Token inválido ou expirado." });

//            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
//            var tipo = principal.FindFirst(ClaimTypes.Role)?.Value;

//            return Ok(new { email, tipo });
//        }

//        [HttpPost("esqueci-senha")]
//        public async Task<IActionResult> EsqueciSenha([FromBody] EsqueciSenhaDto dto)
//        {
//            if (dto == null || string.IsNullOrWhiteSpace(dto.Email))
//                return BadRequest("Email inválido.");

//            var email = dto.Email;

//            // Aqui você coloca a lógica para buscar usuário pelo email,
//            // gerar token e enviar email.

//            // Exemplo:
//            var cliente = await _clienteService.GetByEmailAsync(email);
//            var admin = await _adminService.GetByEmailAsync(email);

//            var usuario = cliente ?? (object)admin;
//            if (usuario == null) return NotFound("E-mail não encontrado.");

//            // Gera token, link e envia email...
//            // ...

//            return Ok("E-mail de redefinição enviado.");
//        }

//        [HttpPost("redefinir-senha")]
//        public async Task<IActionResult> RedefinirSenha([FromBody] RedefinirSenhaDto dto)
//        {
//            var tokenValido = await _resetTokenService.ObterPorTokenAsync(dto.Token);
//            if (tokenValido == null)
//                return BadRequest("Token inválido ou expirado.");

//            // Verifica se é cliente ou admin
//            var cliente = await _clienteService.GetByEmailAsync(tokenValido.Email);
//            var admin = await _adminService.GetByEmailAsync(tokenValido.Email);

//            var novaSenhaCriptografada = BCrypt.Net.BCrypt.HashPassword(dto.NovaSenha);

//            if (cliente != null)
//            {
//                cliente.Senha = novaSenhaCriptografada;
//                await _clienteService.UpdateAsync(cliente.Id, cliente);
//            }
//            else if (admin != null)
//            {
//                admin.Senha = novaSenhaCriptografada;
//                await _adminService.UpdateAsync(admin.Id, admin);
//            }
//            else
//            {
//                return NotFound("Usuário não encontrado.");
//            }

//            await _resetTokenService.RemoverTokenAsync(dto.Token);


//            return Ok("Senha redefinida com sucesso.");
//        }


//        [HttpGet("teste-email")]
//        public async Task<IActionResult> TesteEmail()
//        {
//            var sucesso = await _emailService.EnviarEmailAsync(
//                "viniciusvribeiro85@gmail.com",
//                "🔧 Teste de E-mail",
//                "<h3>Este é um teste de envio de e-mail via C#</h3>"
//            );

//            return Ok(new { sucesso });
//        }
//    }
//}


using LojaMoveis.DTO;
using LojaMoveis.Models;
using LojaMoveis.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LojaMoveis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AdminService _adminService;
        private readonly ClienteService _clienteService;
        private readonly EmailService _emailService;
        private readonly ResetTokenService _resetTokenService;
        private readonly TokenService _tokenService;

        public LoginController(AdminService adminService, ClienteService clienteService, EmailService emailService, ResetTokenService resetTokenService, TokenService tokenService)
        {
            _adminService = adminService;
            _clienteService = clienteService;
            _emailService = emailService;
            _resetTokenService = resetTokenService;
            _tokenService = tokenService;
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

            var token = TokenService.GenerateToken(id, tipo, email);

            Response.Cookies.Append("jwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(2)
            });

            return Ok(new { tipo, email });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwtToken");
            return Ok(new { mensagem = "Logout realizado com sucesso." });
        }

        [HttpGet("perfil")]
        [Authorize]
        public async Task<IActionResult> Perfil()
        {
            var token = Request.Cookies["jwtToken"];

            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { mensagem = "Usuário não autenticado." });

            var principal = TokenService.ValidarToken(token);

            if (principal == null)
                return Unauthorized(new { mensagem = "Token inválido ou expirado." });

            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            var tipo = principal.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new { email, tipo });
        }

        [HttpPost("esqueci-senha")]
        public async Task<IActionResult> EsqueciSenha([FromBody] EsqueciSenhaDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("Email inválido.");

            var email = dto.Email;

            var cliente = await _clienteService.GetByEmailAsync(email);
            var admin = await _adminService.GetByEmailAsync(email);

            var usuario = cliente ?? (object)admin;
            if (usuario == null) return NotFound("E-mail não encontrado.");

            var token = Guid.NewGuid().ToString();

            await _resetTokenService.CriarAsync(new ResetToken
            {
                Email = email,
                Token = token,  
                ExpiraEm = DateTime.UtcNow.AddHours(1)
            });

            var link = $"http://localhost:5173/redefinir-senha/{token}";
            var corpo = $"<p>Você solicitou redefinição de senha.</p><p><a href='{link}'>Clique aqui para redefinir</a></p>";

            var enviado = await _emailService.EnviarEmailAsync(email, "Redefinir senha", corpo);
            if (!enviado)
                return StatusCode(500, "Erro ao enviar e-mail.");

            return Ok("E-mail de redefinição enviado.");
        }

        [HttpPost("redefinir-senha")]
        public async Task<IActionResult> RedefinirSenha([FromBody] RedefinirSenhaDto dto)
        {
            var tokenValido = await _resetTokenService.ObterPorTokenAsync(dto.Token);
            if (tokenValido == null)
                return BadRequest("Token inválido ou expirado.");

            var cliente = await _clienteService.GetByEmailAsync(tokenValido.Email);
            var admin = await _adminService.GetByEmailAsync(tokenValido.Email);

            var novaSenhaCriptografada = BCrypt.Net.BCrypt.HashPassword(dto.NovaSenha);

            if (cliente != null)
            {
                cliente.Senha = novaSenhaCriptografada;
                await _clienteService.UpdateAsync(cliente.Id, cliente);
            }
            else if (admin != null)
            {
                admin.Senha = novaSenhaCriptografada;
                await _adminService.UpdateAsync(admin.Id, admin);
            }
            else
            {
                return NotFound("Usuário não encontrado.");
            }

            await _resetTokenService.RemoverTokenAsync(dto.Token);

            return Ok("Senha redefinida com sucesso.");
        }

        [HttpGet("teste-email")]
        public async Task<IActionResult> TesteEmail()
        {
            var sucesso = await _emailService.EnviarEmailAsync(
                "viniciusvribeiro85@gmail.com",
                "🔧 Teste de E-mail",
                "<h3>Este é um teste de envio de e-mail via C#</h3>"
            );

            return Ok(new { sucesso });
        }
    }
}

