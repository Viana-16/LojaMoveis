using LojaMoveis.Models;
using LojaMoveis.Services;
using Microsoft.AspNetCore.Mvc;

namespace LojaMoveis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClienteController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Cliente cliente)
        {
            var existente = await _clienteService.GetByEmailAsync(cliente.Email);
            if (existente != null)
                return BadRequest("Email já cadastrado.");

            await _clienteService.CreateAsync(cliente);
            return Ok("Cliente registrado com sucesso.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Cliente login)
        {
            var cliente = await _clienteService.GetByEmailAsync(login.Email);
            if (cliente == null || cliente.Senha != login.Senha)
                return Unauthorized("Email ou senha inválidos.");

            return Ok(cliente); // No futuro aqui vai um JWT
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> Get(string id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);
            if (cliente == null)
                return NotFound();

            return cliente;
        }
    }
}
