using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API;

[ApiController]
[Route("api/folha")]
public class ONGController : ControllerBase
{
    private readonly AppDataContext _ctx;

    public ONGController(AppDataContext ctx)
    {
        _ctx = ctx;
    }
    private static List<Folha> folhas = new List<Folha>();

    //GET: api/ong/listar
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

    //GET: api/ong/buscar/{bolacha}
    [HttpGet]
    [Route("buscar/{cpf}/{mes}/{ano}")]
    // public IActionResult Buscar([FromRoute] string nome)
    // {
    //     try
    //     {
    //         Folha? folhaCadastrada =
    //             _ctx.Folhas.Include(x => x.Folha)
    //             .FirstOrDefault(x => x.Nome == nome);
    //         if (ongCadastrada != null)
    //         {
    //             return Ok(ongCadastrada);
    //         }
    //         return NotFound();
    //     }
    //     catch (Exception e)
    //     {
    //         return BadRequest(e.Message);
    //     }



    //POST: api/ong/cadastrar
    [HttpPost]
    [Route("cadastrar")]
    public IActionResult Cadastrar([FromBody] Folha folha)
    {
        try
        {
            _ctx.Folhas.Add(folha);
            _ctx.SaveChanges();
            return Created("", folha);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }
}
