using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API;

[ApiController]
[Route("api/folha")]
public class FolhaController : ControllerBase
{
    private readonly AppDataContext _ctx;

    public FolhaController(AppDataContext ctx)
    {
        _ctx = ctx;
    }
    private static List<Folha> folhas = new List<Folha>();

    //GET: api/folha/listar
    [HttpGet]
    [Route("listar")]
    public IActionResult Listar()
    {
        try
        {
            List<Folha> folhas = _ctx.Folhas.Include(x => x.Funcionario).ToList();
            return folhas.Count == 0 ? NotFound() : Ok(folhas);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // GET: api/folha/buscar/{cpf}/{mes}/{ano}
    [HttpGet]
    [Route("buscar/{cpf}/{mes}/{ano}")]
    public IActionResult Buscar(string cpf, int mes, int ano)
    {
        try
        {
            Folha folha = _ctx.Folhas.Include(x => x.Funcionario).FirstOrDefault(x => x.Funcionario.CPF == cpf && x.Mes == mes && x.Ano == ano);
            return folha != null ? Ok(folha) : NotFound();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }



    //POST: api/ong/cadastrar
    [HttpPost]
    [Route("cadastrar")]
    public IActionResult Cadastrar([FromBody] Folha folha)
    {
        try
        {
            // Cálculo do Salário Bruto
            folha.SalarioBruto = folha.Quantidade * folha.Valor;

            // Cálculo do Imposto de Renda (IRRF)
            if (folha.SalarioBruto <= 1903.98)
            {
                folha.ImpostoIRRF = 0;
            }
            else if (folha.SalarioBruto <= 2826.65)
            {
                folha.ImpostoIRRF = (folha.SalarioBruto * 7.5 / 100) - 142.80;
            }
            else if (folha.SalarioBruto <= 3751.05)
            {
                folha.ImpostoIRRF = (folha.SalarioBruto * 15 / 100) - 354.80;
            }
            else if (folha.SalarioBruto <= 4664.68)
            {
                folha.ImpostoIRRF = (folha.SalarioBruto * 22.5 / 100) - 636.13;
            }
            else
            {
                folha.ImpostoIRRF = (folha.SalarioBruto * 27.5 / 100) - 869.36;
            }

            // Cálculo do INSS
            if (folha.SalarioBruto <= 1693.72)
            {
                folha.ImpostoINSS = folha.SalarioBruto * 8 / 100;
            }
            else if (folha.SalarioBruto <= 2822.90)
            {
                folha.ImpostoINSS = folha.SalarioBruto * 9 / 100;
            }
            else if (folha.SalarioBruto <= 5645.80)
            {
                folha.ImpostoINSS = folha.SalarioBruto * 11 / 100;
            }
            else
            {
                folha.ImpostoINSS = 621.03; // valor fixo
            }

            // Cálculo do FGTS
            folha.ImpostoFGTS = folha.SalarioBruto * 8 / 100;

            // Cálculo do Salário Líquido
            folha.SalarioLiquido = folha.SalarioBruto - folha.ImpostoIRRF - folha.ImpostoINSS;

            // Adicionar a folha ao contexto e salvar no banco de dados
            _ctx.Folhas.Add(folha);
            _ctx.SaveChanges();

            return Created("", folha);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

}
