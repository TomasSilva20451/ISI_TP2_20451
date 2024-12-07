using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class TransacaoController : ControllerBase
{
    private readonly BancoContext _context;

    public TransacaoController(BancoContext context)
    {
        _context = context;
    }

    // GET /api/Transacao
    [HttpGet]
    public ActionResult<IEnumerable<Transacao>> GetTransacoes()
    {
        var transacoes = _context.Transacoes
            .Include(t => t.Conta)
            .ToList();
        return Ok(transacoes);
    }

    // GET /api/Transacao/{id}
    [HttpGet("{id}")]
    public ActionResult<Transacao> GetTransacaoById(int id)
    {
        var transacao = _context.Transacoes
            .Include(t => t.Conta)
            .FirstOrDefault(t => t.TransacaoId == id);

        if (transacao == null)
            return NotFound();

        return Ok(transacao);
    }

    // POST /api/Transacao
    // Cria uma nova transação (depósito, levantamento, etc.)
    [HttpPost]
    public ActionResult<Transacao> CreateTransacao(Transacao transacao)
    {
        var conta = _context.Contas.Find(transacao.ContaId);
        if (conta == null) return BadRequest("Conta não encontrada.");

        // Dependendo da lógica de negócio, definir EstadoTransacao, TipoTransacao, etc.
        // Por exemplo, transações grandes podem começar como 'Pendente', pequenas como 'Aprovadas'
        // Aqui é apenas um CRUD simples, sem lógica adicional.

        _context.Transacoes.Add(transacao);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetTransacaoById), new { id = transacao.TransacaoId }, transacao);
    }

    // PUT /api/Transacao/{id}
    // Atualiza alguma informação da transação (por exemplo, estado da transação)
    [HttpPut("{id}")]
    public ActionResult UpdateTransacao(int id, Transacao transacaoAtualizada)
    {
        // Verifica se o ID da URL corresponde ao ID do objeto enviado
        if (id != transacaoAtualizada.TransacaoId)
            return BadRequest("O ID da transação na URL não corresponde ao ID no corpo da requisição.");

        // Busca a transação existente no banco de dados
        var transacaoExistente = _context.Transacoes.Find(id);
        if (transacaoExistente == null)
            return NotFound("Transação não encontrada.");

        // Validações adicionais
        if (transacaoAtualizada.Valor <= 0)
            return BadRequest("O valor da transação deve ser maior que zero.");

        // Atualiza apenas os campos permitidos
        transacaoExistente.EstadoTransacao = transacaoAtualizada.EstadoTransacao ?? transacaoExistente.EstadoTransacao; // Atualiza somente se não for nulo
        transacaoExistente.TipoTransacao = transacaoAtualizada.TipoTransacao ?? transacaoExistente.TipoTransacao; // Atualiza somente se não for nulo
        transacaoExistente.Valor = transacaoAtualizada.Valor;

        // Adiciona lógica extra (se necessário)
        // Exemplo: Validar se o novo estado é válido, ou ajustar dados com base no tipo de transação

        try
        {
            _context.SaveChanges(); // Salva as alterações no banco de dados
            return NoContent(); // Retorna 204 No Content em caso de sucesso
        }
        catch (Exception ex)
        {
            // Captura erros e retorna uma mensagem detalhada
            return StatusCode(500, $"Erro ao atualizar a transação: {ex.Message}");
        }
    }

    // DELETE /api/Transacao/{id}
    [HttpDelete("{id}")]
    public ActionResult DeleteTransacao(int id)
    {
        var transacao = _context.Transacoes.Find(id);
        if (transacao == null)
            return NotFound();

        _context.Transacoes.Remove(transacao);
        _context.SaveChanges();
        return NoContent();
    }
}