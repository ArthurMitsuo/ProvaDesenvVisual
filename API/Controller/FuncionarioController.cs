namespace API;
using Microsoft.AspNetCore.Mvc;
using API.Data;

[ApiController]
[Route("api/funcionario")]
public class FuncionarioController : ControllerBase
{
    private readonly AppDataContext _context;
    public FuncionarioController(AppDataContext context){
        _context = context;
    }

    //Lista TODOS os funcionários
    [HttpGet]
    [Route("listar")]
    public IActionResult Listar(){
        try
        {
            List<Funcionario>? funcionario = _context?.Funcionarios?.ToList();
            if(funcionario != null){
                return Ok(funcionario);
            }else{
                return NotFound("Nenhum funcionário cadastrado ainda");
            }
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    //Cadastra UM funcionario
    [HttpPost]
    [Route("cadastrar")]
    public IActionResult Cadastrar([FromBody] Funcionario funcionario){
        try
        {
            _context.Add(funcionario);
            _context.SaveChanges();
            return Created("", funcionario);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

}
