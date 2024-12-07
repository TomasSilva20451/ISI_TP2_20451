public class Emprestimo
{
    public int EmprestimoId { get; set; }
    public int ContaId { get; set; }
    public Conta? Conta { get; set; }
    public decimal ValorOriginal { get; set; }
    public decimal SaldoDevedor { get; set; }
    public decimal TaxaJuros { get; set; }
    public string Estado { get; set; } = "Ativo"; // "Ativo", "Liquidado"
}
