namespace API;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            List<Folha>? folhas = _context?.Folhas?.Include(f => f.Funcionario).ToList();
                
            if(folhas != null){
                return Ok(folhas);
            } else{
                return NotFound("404 - Nenhuma folha encontrada");
            }   
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    //Lista folhas por /{cpf}/{mes}/{ano}
    [HttpGet]
    [Route("listar/{cpf}/{mes}/{ano}")]
    public IActionResult ListarEspecifico([FromRoute] string cpf, [FromRoute] int mes, [FromRoute] int ano){
        try
        {
            Folha? folha = _context?.Folhas?.Include(f => f.Funcionario).FirstOrDefault(x => x.Funcionario.Cpf == cpf && x.Ano == ano && x.Mes == mes);

            if(folha != null){
                return Ok(folha);
            }else{
                return NotFound("404");
            }
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
            Folha? folhaCadastro = folha;

            if(listaFuncionario != null){
                foreach(Funcionario func in listaFuncionario){
                    if(func.FuncionarioId == idFuncionario){
                        funcionarioExiste = true;
                        folhaCadastro.Funcionario = func;
                    }
                }
            }else{
                return NotFound("404 - Nenhum funcionário encontrado");
            }

            //Chama a função de cálculo de valores que retorna eles em lista
            List<double?>? lista = CalculaValores(folhaCadastro);

            folhaCadastro.SalarioBruto = lista?[0];
            folhaCadastro.ImpostoIrrf = lista?[1];
            folhaCadastro.ImpostoInss = lista?[2];
            folhaCadastro.ImpostoFgts = lista?[3];
            folhaCadastro.SalarioLiquido = lista?[4];


            //Verifica se p funcionário existe, se ele existir, o valor vai ser true;
            if(funcionarioExiste == false){
               return NotFound("404 - Funcionário não encontrado"); 
            }else{
                _context?.Add(folhaCadastro);
                _context?.SaveChanges();
                return Created("", folhaCadastro);
            }            
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    [NonAction]
    public List<double?>? CalculaValores(Folha folha){
        List<double?>? listaValores = new List<double?>();
        //Calcula o Salario Bruto
        double? SalarioBruto = folha.Valor * folha.Quantidade;

        listaValores.Add(SalarioBruto);
        //Calcula o Imposto de Renda
        double? impostoIrrf = 0;
        
        if (SalarioBruto > 1000 && SalarioBruto <= 1903.98){
            impostoIrrf = 0;
        }else if(SalarioBruto > 1903.98 && SalarioBruto <= 2826.65){
            impostoIrrf = 0.075;
        }else if(SalarioBruto > 2826.65 && SalarioBruto <= 3751.05){
            impostoIrrf = 0.15;
        }else if(SalarioBruto > 3751.05 && SalarioBruto <= 4664.68){
            impostoIrrf = 0.225;
        }else if(SalarioBruto > 4664.68){
            impostoIrrf = 0.227;
        }
        double? irrfCalculado;

        if(impostoIrrf == 0){
            irrfCalculado = SalarioBruto;
        }else{
            irrfCalculado = SalarioBruto * impostoIrrf;
        }

        listaValores.Add(irrfCalculado);
        //Calcula o Valor do INSS
        double? inss=0, inssCalculado;
        if(SalarioBruto <= 1693.72){
            inss = 0.08;
        }else if(SalarioBruto > 1693.72 && SalarioBruto <= 2822.90){
            inss = 0.09;
        }else if(SalarioBruto > 2822.90 && SalarioBruto <= 5645.80){
            inss = 0.11;
        }else if(SalarioBruto > 5645.80){
            inss = 621.03;
        }

        if(SalarioBruto < 5645.81){
            inssCalculado = SalarioBruto * inss;
        }else{
            inssCalculado = inss;
        }

        listaValores.Add(inssCalculado);

        //Calcula o Valor do FGTS
        double? fgtsCalculado = SalarioBruto * 0.08;

        listaValores.Add(fgtsCalculado);
        //Calcula o Valor do Salário Líquido
        double? salarioLiquido = SalarioBruto - irrfCalculado - inssCalculado;
        listaValores.Add(salarioLiquido);

        return listaValores;
    }

}

 
/*
COMANDOS MIGRATIONS
dotnet ef migrations add NomeMigracao
dotnet ef database update
*/