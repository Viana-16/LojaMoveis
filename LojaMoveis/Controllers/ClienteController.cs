using LojaMoveis.Models;
using LojaMoveis.Services;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

namespace LojaMoveis.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    //public class ClienteController : ControllerBase
    //{
    //    private readonly ClienteService _clienteService;

    //    public ClienteController(ClienteService clienteService)
    //    {
    //        _clienteService = clienteService;
    //    }

    //    // POST: api/Cliente
    //    [HttpPost]
    //    public async Task<IActionResult> Post([FromBody] Cliente cliente)
    //    {
    //        await _clienteService.CreateAsync(cliente);
    //        return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
    //    }

    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClienteController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Cliente cliente)
        {
            // Verificar se já existe um cliente com o mesmo e-mail
            var clienteExistente = await _clienteService.GetByEmailAsync(cliente.Email);
            if (clienteExistente != null)
            {
                return Conflict("E-mail já cadastrado.");
            }

            // Criptografar a senha
            cliente.Senha = BCrypt.Net.BCrypt.HashPassword(cliente.Senha);

            await _clienteService.CreateAsync(cliente);
            return Ok(cliente);
        }

        // GET: api/Cliente
        [HttpGet]
        public async Task<ActionResult<List<Cliente>>> Get()
        {
            var clientes = await _clienteService.GetAllAsync();
            return Ok(clientes);
        }

        // GET: api/Cliente/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetById(string id)
        {
            var cliente = await _clienteService.GetByIdAsync(id);
            if (cliente is null)
                return NotFound();

            return Ok(cliente);
        }
    }
}
