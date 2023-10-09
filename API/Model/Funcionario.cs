namespace API;
public class Funcionario
{
    public Funcionario() => CriadoEm = DateTime.Now;

    public int FuncionarioId { get; set; }
    public string? Nome { get; set; }
    public string? Cpf { get; set; }
    public DateTime CriadoEm { get; set; }
}
