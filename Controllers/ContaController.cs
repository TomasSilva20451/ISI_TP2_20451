using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ContaController : ControllerBase
{
    private readonly BancoContext _context;

    public ContaController(BancoContext context)
    {
        _context = context;
    }

    // GET /api/Conta
    // Lista todas as contas
    [HttpGet]
    public ActionResult<IEnumerable<Conta>> GetContas()
    {
        var contas = _context.Contas
            .Include(c => c.Cliente)       // Incluir o cliente relacionado
            .Include(c => c.Transacoes)    // Opcional: incluir transações se necessário
            .Include(c => c.Emprestimos)   // Opcional: incluir empréstimos se necessário
            .ToList();

        return Ok(contas);
    }

    // GET /api/Conta/{id}
    // Obtém uma conta específica pelo ID
    [HttpGet("{id}")]
    public ActionResult<Conta> GetContaById(int id)
    {
        var conta = _context.Contas
            .Include(c => c.Cliente)
            .Include(c => c.Transacoes)
            .Include(c => c.Emprestimos)
            .FirstOrDefault(c => c.ContaId == id);

        if (conta == null)
            return NotFound();

        return Ok(conta);
    }

    // POST /api/Conta
    // Cria uma nova conta
    [HttpPost]
    public ActionResult<Conta> CreateConta(Conta conta)
    {
        var cliente = _context.Clientes.Find(conta.ClienteId);
        if (cliente == null)
        {
            return BadRequest("Cliente não encontrado.");
        }

        conta.Cliente = cliente;
        _context.Contas.Add(conta);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetContaById), new { id = conta.ContaId }, conta);
    }

    // PUT /api/Conta/{id}
    // Atualiza dados de uma conta existente
    [HttpPut("{id}")]
    public ActionResult UpdateConta(int id, Conta contaAtualizada)
    {
        if (id != contaAtualizada.ContaId)
            return BadRequest("ID da conta inconsistente.");

        var contaExistente = _context.Contas.Find(id);
        if (contaExistente == null)
            return NotFound();

        // Atualiza os campos que podem ser modificados
        contaExistente.TipoConta = contaAtualizada.TipoConta;
        contaExistente.Ativa = contaAtualizada.Ativa;
        // Normalmente não se atualiza o saldo direto aqui, a não ser que seja a lógica do seu domínio
        // Também poderia atualizar o ClienteId, se fizer sentido.

        _context.SaveChanges();
        return NoContent(); // 204 No Content
    }

    // DELETE /api/Conta/{id}
    // Remove uma conta existente
    [HttpDelete("{id}")]
    public ActionResult DeleteConta(int id)
    {
        var conta = _context.Contas.Find(id);
        if (conta == null)
            return NotFound();

        _context.Contas.Remove(conta);
        _context.SaveChanges();

        return NoContent(); // 204 No Content
    }
}