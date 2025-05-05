using LojaMoveis.DTO;
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
        public async Task<ActionResult<List<Produto>>> Get()
        {
            var produtos = await _produtoService.GetAsync();
            return Ok(produtos);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Produto>> GetByIdAsync(string id)
        {
            var produto = await _produtoService.GetByIdAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult<Produto>> Create(Produto produto)
        {
            await _produtoService.CreateAsync(produto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = produto.Id }, produto);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Produto produtoAtualizado)
        {
            var produto = await _produtoService.GetByIdAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            produtoAtualizado.Id = id;

            await _produtoService.UpdateAsync(id, produtoAtualizado);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var produto = await _produtoService.GetByIdAsync(id);

            if (produto == null)
            {
                return NotFound();
            }

            await _produtoService.DeleteAsync(id);
            return NoContent();
        }
    }
}

