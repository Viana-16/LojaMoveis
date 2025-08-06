using LojaMoveis.Models;
using LojaMoveis.Services;
using Microsoft.AspNetCore.Mvc;

namespace LojaMoveis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnderecoController : ControllerBase
    {
        private readonly EnderecoService _service;

        public EnderecoController(EnderecoService service)
        {
            _service = service;
        }

        // GET: api/Endereco/{usuarioId}
        [HttpGet("{usuarioId}")]
        public async Task<ActionResult<List<Endereco>>> GetPorUsuario(string usuarioId)
        {
            var enderecos = await _service.GetPorUsuario(usuarioId);
            return Ok(enderecos);
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<Endereco>> GetPorId(string id)
        {
            var endereco = await _service.GetPorId(id);
            if (endereco == null)
                return NotFound("Endereço não encontrado");

            return Ok(endereco);
        }



        // POST: api/Endereco
        [HttpPost]
        public async Task<ActionResult<Endereco>> Criar(Endereco novo)
        {
            if (string.IsNullOrWhiteSpace(novo.UsuarioId) || string.IsNullOrWhiteSpace(novo.TextoEndereco))
            {
                return BadRequest("Dados inválidos.");
            }

            var criado = await _service.Criar(novo);
            return CreatedAtAction(nameof(GetPorUsuario), new { usuarioId = criado.UsuarioId }, criado);
        }
        // PUT: api/Endereco/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(string id, [FromBody] Endereco enderecoAtualizado)
        {
            if (string.IsNullOrWhiteSpace(id) || enderecoAtualizado == null)
                return BadRequest("Dados inválidos.");

            enderecoAtualizado.Id = id; // Garante que o ID não seja nulo
            await _service.Atualizar(id, enderecoAtualizado);
            return NoContent();
        }

        // DELETE: api/Endereco/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("ID inválido.");

            await _service.Remover(id);
            return NoContent();
        }

    }
}
