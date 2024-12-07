using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class PedidoEmprestimoController : ControllerBase
{
    private readonly BancoContext _context;

    public PedidoEmprestimoController(BancoContext context)
    {
        _context = context;
    }

    // GET /api/PedidoEmprestimo
    [HttpGet]
    public ActionResult<IEnumerable<PedidoEmprestimo>> GetPedidosEmprestimo()
    {
        var pedidos = _context.PedidosEmprestimo
            .Include(p => p.Conta)
            .ToList();
        return Ok(pedidos);
    }

    // GET /api/PedidoEmprestimo/{id}
    [HttpGet("{id}")]
    public ActionResult<PedidoEmprestimo> GetPedidoEmprestimoById(int id)
    {
        var pedido = _context.PedidosEmprestimo
            .Include(p => p.Conta)
            .FirstOrDefault(p => p.PedidoEmprestimoId == id);

        if (pedido == null)
            return NotFound();

        return Ok(pedido);
    }

    // POST /api/PedidoEmprestimo
    // Cria um novo pedido de empréstimo
    [HttpPost]
    public ActionResult<PedidoEmprestimo> CreatePedidoEmprestimo(PedidoEmprestimo pedido)
    {
        var conta = _context.Contas.Find(pedido.ContaId);
        if (conta == null) return BadRequest("Conta não encontrada.");

        // Atribui a conta carregada do banco
        pedido.Conta = conta;

        // Estado inicial como 'Pendente' (por exemplo)
        pedido.EstadoPedido = "Pendente";
        _context.PedidosEmprestimo.Add(pedido);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetPedidoEmprestimoById), new { id = pedido.PedidoEmprestimoId }, pedido);
    }

    // PUT /api/PedidoEmprestimo/{id}
    // Atualiza o estado do pedido, por exemplo, para "Aprovado" ou "Rejeitado"
    [HttpPut("{id}")]
    public IActionResult UpdatePedidoEmprestimo(int id, PedidoEmprestimo updatedPedido)
    {
        if (id != updatedPedido.PedidoEmprestimoId)
            return BadRequest("O ID do pedido não corresponde ao ID da URL.");

        var pedido = _context.PedidosEmprestimo.Find(id);
        if (pedido == null)
            return NotFound("Pedido de empréstimo não encontrado.");

        // Atualiza os campos necessários
        pedido.ContaId = updatedPedido.ContaId;
        pedido.ValorSolicitado = updatedPedido.ValorSolicitado;
        pedido.DataPedido = updatedPedido.DataPedido;
        pedido.EstadoPedido = updatedPedido.EstadoPedido;

        _context.PedidosEmprestimo.Update(pedido);
        _context.SaveChanges();

        return NoContent(); // Retorna 204 indicando sucesso sem conteúdo
    }

    // DELETE /api/PedidoEmprestimo/{id}
    [HttpDelete("{id}")]
    public ActionResult DeletePedidoEmprestimo(int id)
    {
        var pedido = _context.PedidosEmprestimo.Find(id);
        if (pedido == null)
            return NotFound();

        _context.PedidosEmprestimo.Remove(pedido);
        _context.SaveChanges();
        return NoContent();
    }
}