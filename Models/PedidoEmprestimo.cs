using System.Text.Json.Serialization;
public class PedidoEmprestimo
{
    public int PedidoEmprestimoId { get; set; }
    public int ContaId { get; set; }

    [JsonIgnore] // Impede que o campo seja validado no corpo da requisição
    public Conta? Conta { get; set; }
    public decimal ValorSolicitado { get; set; }
    public DateTime DataPedido { get; set; } = DateTime.UtcNow;
    public string EstadoPedido { get; set; } = "Pendente"; // "Pendente", "Aprovado", "Rejeitado"
}