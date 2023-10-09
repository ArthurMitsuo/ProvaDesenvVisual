using API;
using Microsoft.EntityFrameworkCore;

namespace API.Data;
public class AppDataContext : DbContext
{
     public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
    {
        
    }

    //Classes que vão se tornar tabelas no banco de dados
    public DbSet<Folha>? Folhas {get; set;}
    public DbSet<Funcionario>? Funcionarios {get; set;}

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Consultar as folhas com carregamento antecipado do funcionário
        modelBuilder.Entity<Folha>()
            .HasOne(f => f.Funcionario);

        base.OnModelCreating(modelBuilder);
    }
}
