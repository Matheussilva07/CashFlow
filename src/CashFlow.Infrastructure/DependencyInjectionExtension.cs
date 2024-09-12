using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure;
public static class DependencyInjectionExtension
{
    //Este é um método de extensão que é usado para colocar novos métodos em classes do próprio sistema e que não temos acesso ao código fonte:
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
       AddDbContext(services, configuration);
       AddRepositories(services);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IExpensesReadOnlyRepository, ExpenseRepository>();
        services.AddScoped<IExpensesWriteOnlyRepository, ExpenseRepository>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");

        var version = new Version(8, 0, 39);
        var serverVersion = new MySqlServerVersion(version);

        services.AddDbContext<CashFlowDBContext>(config => config.UseMySql(connectionString, serverVersion));
    }
}
