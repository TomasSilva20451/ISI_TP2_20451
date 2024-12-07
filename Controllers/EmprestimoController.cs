using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class EmprestimoController : ControllerBase
{
    private readonly BancoContext _context;

    public EmprestimoController(BancoContext context)
    {
        _context = context;
    }

    // GET /api/Emprestimo
    [HttpGet]
    public ActionResult<IEnumerable<Emprestimo>> GetEmprestimos()
    {
        var emprestimos = _context.Emprestimos
            .Include(e => e.Conta) // Se quiser incluir informações da conta associada
            .ToList();
        return Ok(emprestimos);
    }

    // GET /api/Emprestimo/{id}
    [HttpGet("{id}")]
    public ActionResult<Emprestimo> GetEmprestimoById(int id)
    {
        var emprestimo = _context.Emprestimos
            .Include(e => e.Conta)
            .FirstOrDefault(e => e.EmprestimoId == id);

        if (emprestimo == null)
            return NotFound();

        return Ok(emprestimo);
    }

    // POST /api/Emprestimo
    [HttpPost]
    public ActionResult<Emprestimo> CreateEmprestimo(Emprestimo emprestimo)
    {
        var conta = _context.Contas.Find(emprestimo.ContaId);
        if (conta == null) return BadRequest("Conta não encontrada.");

        // Atribui a conta carregada do banco
        emprestimo.Conta = conta;

        _context.Emprestimos.Add(emprestimo);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetEmprestimoById), new { id = emprestimo.EmprestimoId }, emprestimo);
    }

    // PUT /api/Emprestimo/{id}
    [HttpPut("{id}")]
    public ActionResult UpdateEmprestimo(int id, Emprestimo emprestimoAtualizado)
    {
        if (id != emprestimoAtualizado.EmprestimoId)
            return BadRequest("ID inconsistente.");

        var emprestimoExistente = _context.Emprestimos.Find(id);
        if (emprestimoExistente == null)
            return NotFound();

        // Atualiza campos permitidos
        emprestimoExistente.SaldoDevedor = emprestimoAtualizado.SaldoDevedor;
        emprestimoExistente.Estado = emprestimoAtualizado.Estado;
        // Ajustar outros campos conforme necessidade

        _context.SaveChanges();
        return NoContent();
    }

    // DELETE /api/Emprestimo/{id}
    [HttpDelete("{id}")]
    public ActionResult DeleteEmprestimo(int id)
    {
        var emprestimo = _context.Emprestimos.Find(id);
        if (emprestimo == null)
            return NotFound();

        _context.Emprestimos.Remove(emprestimo);
        _context.SaveChanges();
        return NoContent();
    }
}