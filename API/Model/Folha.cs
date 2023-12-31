﻿namespace API;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Folha
{
    public Folha() => CriadoEm = DateTime.Now;

    public int FolhaId { get; set; }
    public double? Valor { get; set; }
    public int? Quantidade { get; set; }
    public int? Mes { get; set; }
    public int? Ano { get; set; }
    public double? SalarioBruto { get; set;}
    public double? ImpostoIrrf { get; set;}
    public double? ImpostoInss { get; set;}
    public double? ImpostoFgts { get; set;}
    public double? SalarioLiquido { get; set;}
    public Funcionario? Funcionario {get;set;}
    [ForeignKey("FuncionarioId")]
    public int FuncionarioId {get;set;}
    public DateTime CriadoEm { get; set; }
}
