using API;
using Microsoft.EntityFrameworkCore;

namespace API.Data;
public class AppDataContext : DbContext
{
     public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
    {
        
    }

    //Classes que vão se tornar tabelas no banco de dados
    //public DbSet<Cargo> Cargos {get; set;}
    //public DbSet<TarefaAtividade> TarefasAtividade {get; set;}

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
