using LojaMoveis.Models;
using LojaMoveis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using LojaMoveis.DTO;
using MongoDB.Bson;
using MongoDB.Driver;

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

        // POST simples (sem imagem)
        [HttpPost]
        public async Task<IActionResult> AdicionarProduto([FromBody] Produto produto)
        {
            await _produtoService.CadastrarProdutoAsync(produto);
            return Ok("Produto adicionado com sucesso.");
        }

        // POST com imagem (Cloudinary)
        [HttpPost("upload")]
        public async Task<IActionResult> AdicionarProdutoComImagens(
    [FromForm] Produto produto,
    IFormFile imagem, // imagem de capa
    List<IFormFile>? imagensExtras, // imagens adicionais
    [FromServices] CloudinaryService cloudinaryService)
        {
            if (imagem == null || imagem.Length == 0)
                return BadRequest("Imagem principal (de capa) não enviada.");

            // Upload da imagem de capa
            var imagemUrl = await cloudinaryService.UploadImagemAsync(imagem);
            if (string.IsNullOrEmpty(imagemUrl))
                return BadRequest("Falha no upload da imagem principal.");

            produto.ImagemUrl = imagemUrl;

            // Inicializa a lista de imagens extras, se necessário
            if (produto.ImagensExtras == null)
                produto.ImagensExtras = new List<string>();

            // Upload das imagens extras (galeria)
            if (imagensExtras != null && imagensExtras.Count > 0)
            {
                foreach (var img in imagensExtras)
                {
                    var url = await cloudinaryService.UploadImagemAsync(img);
                    if (!string.IsNullOrEmpty(url))
                        produto.ImagensExtras.Add(url);
                }
            }

            await _produtoService.CadastrarProdutoAsync(produto);
            return Ok(new
            {
                mensagem = "Produto cadastrado com sucesso!",
                produto
            });
        }


        [HttpGet("aleatorios/{quantidade}/{excetoId}")]
        public async Task<ActionResult<List<Produto>>> GetAleatorios(int quantidade, string excetoId)
        {
            var todos = await _produtoService.GetAsync();
            var aleatorios = todos
                .Where(p => p.Id != excetoId)
                .OrderBy(p => Guid.NewGuid())
                .Take(quantidade)
                .ToList();

            return Ok(aleatorios);
        }



        // GET todos os produtos
        [HttpGet]
        public async Task<ActionResult<List<Produto>>> Get()
        {
            var produtos = await _produtoService.GetAsync();
            return Ok(produtos);
        }

        // GET por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetById(string id)
        {
            var produto = await _produtoService.GetByIdAsync(id);
            if (produto == null)
                return NotFound("Produto não encontrado.");
            return Ok(produto);
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Produto produtoAtualizado)
        {
            var existente = await _produtoService.GetByIdAsync(id);
            if (existente == null)
                return NotFound("Produto não encontrado.");

            await _produtoService.UpdateAsync(id, produtoAtualizado);
            return Ok("Produto atualizado com sucesso.");
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var produto = await _produtoService.GetByIdAsync(id);
            if (produto == null)
                return NotFound("Produto não encontrado.");

            await _produtoService.DeleteAsync(id);
            return Ok("Produto removido com sucesso.");
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarProdutos([FromQuery] string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return BadRequest("Parâmetro de busca vazio.");

            var produtos = await _produtoService.BuscarProdutosAsync(search);
            return Ok(produtos);
        }


    }
}