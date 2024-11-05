using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess;
internal class CashFlowDBContext : DbContext
{

    //A sintaxe abaixo quer dizer que a classe base que é a DbContext está recebendo os parametros desse construtor
    public CashFlowDBContext(DbContextOptions options) : base(options) { }
   
    //É com esta propriedade que acessamos nossa tabela e elas devem ter o mesmo nome
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<User> Users { get; set; }
 
}
