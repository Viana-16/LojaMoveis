using LojaMoveis.Models;
using LojaMoveis.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LojaMoveis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _produtoService;
        private readonly AdminService _adminService;

        public ProdutoController(ProdutoService produtoService, AdminService adminService)
        {
            _produtoService = produtoService;
            _adminService = adminService;
        }

        // POST: api/Produto
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Produto produto, [FromQuery] string emailAdmin, [FromQuery] string senhaAdmin)
        {
            // Verifica se o administrador existe com a senha e email fornecidos
            var admin = await _adminService.LoginAsync(emailAdmin, senhaAdmin);

            if (admin == null)
            {
                return Unauthorized("Apenas administradores podem adicionar produtos.");
            }

            // Caso o admin seja validado, o produto será adicionado
            await _produtoService.CreateAsync(produto);
            return Ok("Produto adicionado com sucesso.");
        }

        // GET: api/Produto
        [HttpGet]
        public async Task<ActionResult<List<Produto>>> Get()
        {
            var produtos = await _produtoService.GetAsync();
            return Ok(produtos);
        }

        // GET: api/Produto/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetById(string id)
        {
            var produto = await _produtoService.GetByIdAsync(id);

            if (produto == null)
                return NotFound("Produto não encontrado.");

            return Ok(produto);
        }

        // PUT: api/Produto/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Produto produtoAtualizado)
        {
            var produtoExistente = await _produtoService.GetByIdAsync(id);
            if (produtoExistente == null)
                return NotFound("Produto não encontrado.");

            await _produtoService.UpdateAsync(id, produtoAtualizado);
            return Ok("Produto atualizado com sucesso.");
        }

        // DELETE: api/Produto/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var produto = await _produtoService.GetByIdAsync(id);
            if (produto == null)
                return NotFound("Produto não encontrado.");

            await _produtoService.DeleteAsync(id);
            return Ok("Produto removido com sucesso.");
        }
    }
}
