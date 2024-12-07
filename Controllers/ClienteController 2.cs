using Microsoft.AspNetCore.Mvc; // Reconhece ApiController, Route, HttpGet, ControllerBase, ActionResult<T>

[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly BancoContext _context;

    public ClienteController(BancoContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Cliente>> GetClientes()
    {
        var clientes = _context.Clientes.ToList();
        return Ok(clientes);
    }

    [HttpGet("{id}")]
    public ActionResult<Cliente> GetClienteById(int id)
    {
        var cliente = _context.Clientes.Find(id);
        if (cliente == null) return NotFound();
        return Ok(cliente);
    }

    [HttpPost]
    public ActionResult<Cliente> CreateCliente(Cliente cliente)
    {
        _context.Clientes.Add(cliente);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetClienteById), new { id = cliente.ClienteId }, cliente);
    }
}