using Microsoft.EntityFrameworkCore;

public class BancoContext : DbContext
{
    public BancoContext(DbContextOptions<BancoContext> options) : base(options) {}

    public DbSet<Cliente> Clientes { get; set; } = null!;
    public DbSet<Conta> Contas { get; set; } = null!;
    public DbSet<Transacao> Transacoes { get; set; } = null!;
    public DbSet<Emprestimo> Emprestimos { get; set; } = null!;
    public DbSet<PedidoEmprestimo> PedidosEmprestimo { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configurações adicionais se necessárias.
    }
}