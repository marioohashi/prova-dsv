namespace API.Models;
public class Folha
{
    public int FolhaId { get; set; }
    public float? Valor { get; set; }
    public int? Quantidade { get; set; }
    public int? Mes { get; set; }
    public int? Ano { get; set; }
    public float? salarioBruto { get; set; }
    public float? ImpostoIRRF { get; set; }
    public float? ImpostoINSS { get; set; }
    public float? ImpostoFGTS { get; set; }
    public float? SalarioLiquido { get; set; }
    public Funcionario? Funcionario { get; set; }
    public int FuncionarioId { get; set; }
}

