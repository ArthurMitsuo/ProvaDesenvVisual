namespace API;
using Microsoft.AspNetCore.Mvc;
using API.Data;

[ApiController]
[Route("api/folha")]
public class FolhaController : ControllerBase
{
    private readonly AppDataContext _context;
    public FolhaController(AppDataContext context){
        _context = context;
    }

    //Lista TODAS as folhas
    [HttpGet]
    [Route("listar")]
    public IActionResult Listar(){
        try
        {
            List<Folha>? folha = _context?.Folhas?.ToList();
            return Ok(folha);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    //Cadastra UMA folha
    [HttpPost]
    [Route("cadastrar")]
    public IActionResult Cadastrar([FromBody] Folha folha){
        try
        {
            //variável booleana para, dentro do loop, indicar se o funcionário indicado existe
            bool? funcionarioExiste = false;

            int? idFuncionario = folha.FuncionarioId;
            List<Funcionario>? listaFuncionario = _context?.Funcionarios?.ToList();

            if(listaFuncionario != null){
                foreach(Funcionario func in listaFuncionario){
                    if(func.FuncionarioId == idFuncionario){
                        funcionarioExiste = true;
                    }
                }
            }else{
                return NotFound("404 - Nenhum funcionário encontrado");
            }

            //Verifica se p funcionário existe, se ele existir, o valor vai ser true;
            if(funcionarioExiste == false){
               return NotFound("404 - Funcionário não encontrado"); 
            }else{
                _context?.Add(folha);
                _context?.SaveChanges();
                return Created("", folha);
            }            
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

}

/*
COMANDOS MIGRATIONS
dotnet ef migrations add NomeMigracao
dotnet ef database update
*/