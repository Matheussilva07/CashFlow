using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess;
public class CashFlowDBContext : DbContext
{
    //É com esta propriedade que acessamos nossa tabela e elas devem ter o mesmo nome
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "";
        var version = new Version();




        optionsBuilder.UseMySql();
    }
}
