using System.Text.Json.Serialization;
public class Cliente
{
    public int ClienteId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string NIF { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Conta> Contas { get; set; } = new List<Conta>();
}
