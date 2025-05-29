using LojaMoveis.Models;
using LojaMoveis.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;

namespace LojaMoveis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _produtoService;
        private readonly AdminService _adminService;
        private readonly IWebHostEnvironment _env;

        public ProdutoController(ProdutoService produtoService, AdminService adminService, IWebHostEnvironment env)
        {
            _produtoService = produtoService;
            _adminService = adminService;
            _env = env;
        }

        // POST: api/Produto
        [HttpPost]
        public async Task<IActionResult> AdicionarProduto([FromBody] Produto produto)
        {
            await _produtoService.CadastrarProdutoAsync(produto);
            return Ok("Produto adicionado com sucesso.");
        }

        // NOVO MÉTODO: POST com imagem
        [HttpPost("com-imagem")]
        public async Task<IActionResult> AdicionarProdutoComImagem([FromForm] Produto produto, IFormFile imagem)
        {
            if (imagem != null && imagem.Length > 0)
            {
                var nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(imagem.FileName);
                var caminhoPasta = Path.Combine(_env.WebRootPath, "imagens");

                if (!Directory.Exists(caminhoPasta))
                    Directory.CreateDirectory(caminhoPasta);

                var caminhoCompleto = Path.Combine(caminhoPasta, nomeArquivo);

                using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                {
                    await imagem.CopyToAsync(stream);
                }

                produto.ImagemUrl = $"imagens/{nomeArquivo}";
            }

            await _produtoService.CadastrarProdutoAsync(produto);
            return Ok("Produto com imagem adicionado com sucesso.");
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
