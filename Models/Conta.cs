using System.Text.Json.Serialization;

public class Conta
{
    public int ContaId { get; set; }
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
    public decimal Saldo { get; set; }
    public string TipoConta { get; set; } = "avista"; // Poderia haver "poupanca" etc.
    public bool Ativa { get; set; } = true;

    public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
    
    [JsonIgnore]
    public ICollection<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();
}