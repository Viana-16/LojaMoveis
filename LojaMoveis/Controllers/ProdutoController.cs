////using LojaMoveis.Models;
////using LojaMoveis.Services;
////using Microsoft.AspNetCore.Mvc;
////using System.Threading.Tasks;
////using System.IO;
////using Microsoft.AspNetCore.Hosting;
////using Microsoft.AspNetCore.Http;
////using System;

////namespace LojaMoveis.Controllers
////{
////    [ApiController]
////    [Route("api/[controller]")]
////    public class ProdutoController : ControllerBase
////    {
////        private readonly ProdutoService _produtoService;
////        private readonly AdminService _adminService;
////        private readonly IWebHostEnvironment _env;

////        public ProdutoController(ProdutoService produtoService, AdminService adminService, IWebHostEnvironment env)
////        {
////            _produtoService = produtoService;
////            _adminService = adminService;
////            _env = env;
////        }

////        // POST: api/Produto
////        [HttpPost]
////        public async Task<IActionResult> AdicionarProduto([FromBody] Produto produto)
////        {
////            await _produtoService.CadastrarProdutoAsync(produto);
////            return Ok("Produto adicionado com sucesso.");
////        }


////        //[HttpPost("upload")]
////        //public async Task<IActionResult> AdicionarProdutoComImagem([FromForm] Produto produto, IFormFile imagem)
////        //{
////        //    if (imagem == null || imagem.Length == 0)
////        //        return BadRequest("Imagem não enviada.");

////        //    var pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagens");
////        //    if (!Directory.Exists(pasta))
////        //        Directory.CreateDirectory(pasta);

////        //    var nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(imagem.FileName);
////        //    var caminhoCompleto = Path.Combine(pasta, nomeArquivo);

////        //    using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
////        //    {
////        //        await imagem.CopyToAsync(stream);
////        //    }

////        //    // URL para acessar a imagem
////        //    produto.ImagemUrl = $"imagens/{nomeArquivo}";

////        //    await _produtoService.CadastrarProdutoAsync(produto);
////        //    return Ok(produto); // Retorna o produto completo
////        //}


////        [HttpPost("upload")]
////        public async Task<IActionResult> AdicionarProdutoComImagem([FromForm] Produto produto, IFormFile imagem, [FromServices] CloudinaryService cloudinaryService)
////        {
////            if (imagem == null || imagem.Length == 0)
////                return BadRequest("Imagem não enviada.");

////            var imagemUrl = await cloudinaryService.UploadImagemAsync(imagem);
////            produto.ImagemUrl = imagemUrl;

////            await _produtoService.CadastrarProdutoAsync(produto);
////            return Ok(produto);
////        }



////        // GET: api/Produto
////        [HttpGet]
////        public async Task<ActionResult<List<Produto>>> Get()
////        {
////            var produtos = await _produtoService.GetAsync();
////            return Ok(produtos);
////        }

////        // GET: api/Produto/{id}
////        [HttpGet("{id}")]
////        public async Task<ActionResult<Produto>> GetById(string id)
////        {
////            var produto = await _produtoService.GetByIdAsync(id);

////            if (produto == null)
////                return NotFound("Produto não encontrado.");

////            return Ok(produto);
////        }

////        // PUT: api/Produto/{id}
////        [HttpPut("{id}")]
////        public async Task<IActionResult> Update(string id, [FromBody] Produto produtoAtualizado)
////        {
////            var produtoExistente = await _produtoService.GetByIdAsync(id);
////            if (produtoExistente == null)
////                return NotFound("Produto não encontrado.");

////            await _produtoService.UpdateAsync(id, produtoAtualizado);
////            return Ok("Produto atualizado com sucesso.");
////        }

////        // DELETE: api/Produto/{id}
////        [HttpDelete("{id}")]
////        public async Task<IActionResult> Delete(string id)
////        {
////            var produto = await _produtoService.GetByIdAsync(id);
////            if (produto == null)
////                return NotFound("Produto não encontrado.");

////            await _produtoService.DeleteAsync(id);
////            return Ok("Produto removido com sucesso.");
////        }


////    }
////}


//using LojaMoveis.Models;
//using LojaMoveis.Services;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using System.IO;
//using System.Threading.Tasks;
//using System;
//using System.Collections.Generic;
//using Newtonsoft.Json;

//namespace LojaMoveis.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class ProdutoController : ControllerBase
//    {
//        private readonly ProdutoService _produtoService;
//        private readonly AdminService _adminService;

//        public ProdutoController(ProdutoService produtoService, AdminService adminService)
//        {
//            _produtoService = produtoService;
//            _adminService = adminService;
//        }

//        // POST simples sem imagem
//        [HttpPost]
//        public async Task<IActionResult> AdicionarProduto([FromBody] Produto produto)
//        {
//            await _produtoService.CadastrarProdutoAsync(produto);
//            return Ok("Produto adicionado com sucesso.");
//        }

//        [HttpPost("upload")]
//        public async Task<IActionResult> AdicionarProdutoComImagens(
//    [FromForm] Produto produto,
//    IFormFile imagem, // imagem principal
//    List<IFormFile>? imagensExtras, // lista de imagens extras
//    [FromServices] CloudinaryService cloudinaryService)
//        {
//            if (imagem == null || imagem.Length == 0)
//                return BadRequest("Imagem principal não enviada.");

//            // Upload imagem principal
//            var imagemUrl = await cloudinaryService.UploadImagemAsync(imagem);
//            if (string.IsNullOrEmpty(imagemUrl))
//                return BadRequest("Falha no upload da imagem principal.");

//            produto.ImagemUrl = imagemUrl;

//            // Upload de imagens extras, se houver
//            if (imagensExtras != null && imagensExtras.Count > 0)
//            {
//                foreach (var img in imagensExtras)
//                {
//                    var url = await cloudinaryService.UploadImagemAsync(img);
//                    if (!string.IsNullOrEmpty(url))
//                        produto.ImagensExtras.Add(url);
//                }
//            }

//            await _produtoService.CadastrarProdutoAsync(produto);
//            return Ok(produto);
//        }


//        // GET: api/Produto
//        [HttpGet]
//        public async Task<ActionResult<List<Produto>>> Get()
//        {
//            var produtos = await _produtoService.GetAsync();
//            return Ok(produtos);
//        }

//        // GET: api/Produto/{id}
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Produto>> GetById(string id)
//        {
//            var produto = await _produtoService.GetByIdAsync(id);
//            if (produto == null)
//                return NotFound("Produto não encontrado.");

//            return Ok(produto);
//        }

//        // PUT: api/Produto/{id}
//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(string id, [FromBody] Produto produtoAtualizado)
//        {
//            var existente = await _produtoService.GetByIdAsync(id);
//            if (existente == null)
//                return NotFound("Produto não encontrado.");

//            await _produtoService.UpdateAsync(id, produtoAtualizado);
//            return Ok("Produto atualizado com sucesso.");
//        }

//        // DELETE: api/Produto/{id}
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(string id)
//        {
//            var produto = await _produtoService.GetByIdAsync(id);
//            if (produto == null)
//                return NotFound("Produto não encontrado.");

//            await _produtoService.DeleteAsync(id);
//            return Ok("Produto removido com sucesso.");
//        }

//    }
//}


using LojaMoveis.Models;
using LojaMoveis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

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


        [HttpPost("{id}/avaliar")]
        public async Task<IActionResult> AvaliarProduto(string id, [FromBody] AvaliacaoRequest request)
        {
            var produto = await _produtoService.GetByIdAsync(id);
            if (produto == null)
                return NotFound("Produto não encontrado.");

            // Atualiza média e quantidade de avaliações
            produto.QtdAvaliacoes = (produto.QtdAvaliacoes ?? 0) + 1;
            produto.AvaliacaoMedia = ((produto.AvaliacaoMedia ?? 0) * (produto.QtdAvaliacoes - 1) + request.Nota) / produto.QtdAvaliacoes;

            // (Opcional) salvar comentários num campo ListaDeComentarios
            produto.Comentarios ??= new List<string>();
            produto.Comentarios.Add(request.Comentario);

            await _produtoService.UpdateAsync(id, produto);
            return Ok(produto);
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

    }
}