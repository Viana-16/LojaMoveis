using LojaMoveis.Models;
using LojaMoveis.Services;
using Microsoft.AspNetCore.Mvc;

namespace LojaMoveis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoService _pedidoService;

        public PedidoController(PedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpPost]
        public async Task<IActionResult> Criar(Pedido pedido)
        {
            try
            {
                await _pedidoService.CriarAsync(pedido);
                return Ok("Pedido criado com sucesso.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar pedido: {ex.Message}");
            }
        }

        [HttpGet("usuario/{email}")]
        public async Task<IActionResult> GetPorUsuario(string email)
        {
            var pedidos = await _pedidoService.GetPorUsuarioAsync(email);
            return Ok(pedidos);
        }

    }
}
