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

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Cliente cliente)
        {
            try
            {
                // Criptografa a senha e cria o cliente
                await _clienteService.CreateAsync(cliente);
                return Ok("Cliente cadastrado com sucesso!");
            }
            catch (Exception ex)
            {
                // Captura o erro de "e-mail já cadastrado" ou outro erro e retorna uma resposta apropriada
                return BadRequest(ex.Message);  // Exibe a mensagem de erro que foi gerada na camada de serviço
            }
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
