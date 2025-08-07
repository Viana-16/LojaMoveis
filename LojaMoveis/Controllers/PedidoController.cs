//using LojaMoveis.Models;
//using LojaMoveis.Services;
//using Microsoft.AspNetCore.Mvc;

//namespace LojaMoveis.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class PedidoController : ControllerBase
//    {
//        private readonly PedidoService _pedidoService;

//        public PedidoController(PedidoService pedidoService)
//        {
//            _pedidoService = pedidoService;
//        }

//        [HttpPost]
//        public async Task<IActionResult> Criar(Pedido pedido)
//        {
//            try
//            {
//                await _pedidoService.CriarAsync(pedido);
//                return Ok("Pedido criado com sucesso.");
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Erro ao criar pedido: {ex.Message}");
//            }
//        }

//        [HttpGet("usuario/{email}")]
//        public async Task<IActionResult> GetPorUsuario(string email)
//        {
//            var pedidos = await _pedidoService.GetPorUsuarioAsync(email);
//            return Ok(pedidos);
//        }

//        [HttpGet("por-email/{email}")]
//        public async Task<ActionResult<IEnumerable<Pedido>>> GetPorEmail(string email)
//        {
//            if (string.IsNullOrWhiteSpace(email))
//                return BadRequest("Email inválido.");

//            var pedidos = await _pedidoService.ObterPorEmail(email);

//            if (pedidos == null || !pedidos.Any())
//                return NotFound("Nenhum pedido encontrado para esse e-mail.");

//            return Ok(pedidos);
//        }


//    }
//}


using LojaMoveis.DTO;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase
{
    private readonly PedidoService _pedidoService;

    public PedidoController(PedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [HttpGet("usuario/{email}")]
    public async Task<IActionResult> GetPorUsuario(string email)
    {
        var pedidos = await _pedidoService.GetPorUsuarioAsync(email);
        return Ok(pedidos);
    }

    [HttpGet("por-email/{email}")]
    public async Task<ActionResult<List<Pedido>>> GetPorEmail(string email)
    {
        var pedidos = await _pedidoService.ObterPorEmail(email);
        if (pedidos == null || pedidos.Count == 0)
            return NotFound();

        return pedidos;
    }

    [HttpPost]
    public async Task<ActionResult> CriarPedido([FromBody] Pedido novo)
    {
        await _pedidoService.Criar(novo);
        return Ok();
    }

    // Exemplo de rota para atualizar um pedido existente
    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(string id, [FromBody] Pedido pedidoAtualizado)
    {
        if (string.IsNullOrEmpty(id) || pedidoAtualizado == null)
            return BadRequest("ID ou dados do pedido inválidos.");

        var resultado = await _pedidoService.Atualizar(id, pedidoAtualizado);

        if (!resultado)
            return NotFound();

        return NoContent();
    }

    //[HttpPost]
    //public async Task<ActionResult<Pedido>> Criar(Pedido novoPedido)
    //{
    //    await _pedidoService.Criar(novoPedido);
    //    return CreatedAtAction(nameof(GetPorEmail), new { email = novoPedido.Email }, novoPedido);
    //}

}

