using LojaMoveis.Models;
using LojaMoveis.Services;
using Microsoft.AspNetCore.Mvc;

namespace LojaMoveis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _produtoService;

        public ProdutoController(ProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Produto>>> Get() =>
            await _produtoService.GetAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> Get(string id)
        {
            var produto = await _produtoService.GetByIdAsync(id);
            if (produto == null)
                return NotFound();

            return produto;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Produto produto)
        {
            await _produtoService.CreateAsync(produto);
            return CreatedAtAction(nameof(Get), new { id = produto.Id }, produto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, Produto produto)
        {
            var existing = await _produtoService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            produto.Id = existing.Id;
            await _produtoService.UpdateAsync(id, produto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var produto = await _produtoService.GetByIdAsync(id);
            if (produto == null)
                return NotFound();

            await _produtoService.DeleteAsync(id);
            return NoContent();
        }
    }
}
