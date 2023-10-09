namespace API.Models;
public class Funcionario
{
    public Funcionario() => CriadoEm = DateTime.Now;
    public int FuncionarioId { get; set; }
    public string? Nome { get; set; }
    public string? CPF { get; set; }
    public DateTime CriadoEm { get; set; }
}