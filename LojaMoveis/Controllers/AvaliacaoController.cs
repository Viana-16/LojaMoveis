using LojaMoveis.Models;
using LojaMoveis.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LojaMoveis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvaliacaoController : ControllerBase
    {
        private readonly AvaliacaoService _service;

        public AvaliacaoController(AvaliacaoService service)
        {
            _service = service;
        }

        // GET api/avaliacao/produto/{idProduto}
        [HttpGet("produto/{idProduto}")]
        public async Task<IActionResult> GetPorProduto(string idProduto)
        {
            var avaliacoes = await _service.GetPorProdutoAsync(idProduto);
            return Ok(avaliacoes);
        }

        // POST api/avaliacao
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] Avaliacao novaAvaliacao)
        {
            if (novaAvaliacao == null || novaAvaliacao.Nota < 1 || novaAvaliacao.Nota > 5 || string.IsNullOrEmpty(novaAvaliacao.IdProduto))
            {
                return BadRequest("Dados inválidos.");
            }

            bool jaAvaliado = await _service.JaAvaliouAsync(novaAvaliacao.IdProduto, novaAvaliacao.ClienteEmail);
            if (jaAvaliado)
            {
                return BadRequest("Cliente já avaliou este produto.");
            }

            novaAvaliacao.DataCriacao = DateTime.UtcNow;
            var criada = await _service.CriarAsync(novaAvaliacao);
            return CreatedAtAction(nameof(GetPorProduto), new { idProduto = criada.IdProduto }, criada);
        }
    }
}
