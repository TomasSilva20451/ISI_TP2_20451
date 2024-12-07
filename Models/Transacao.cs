using System.Text.Json.Serialization;
public class Transacao
{
    public int TransacaoId { get; set; }
    public int ContaId { get; set; }

    [JsonIgnore] // Garante que o campo não é esperado no JSON de entrada
    public Conta? Conta { get; set; }

    public string TipoTransacao { get; set; } = string.Empty; // "deposito", "levantamento"
    public decimal Valor { get; set; }
    public DateTime DataHora { get; set; } = DateTime.UtcNow;
    public string EstadoTransacao { get; set; } = "Pendente"; // "Pendente", "Aprovada", "Rejeitada"
}