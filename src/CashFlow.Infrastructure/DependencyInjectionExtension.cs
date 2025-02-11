using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.DataAccess.Repositories;
using CashFlow.Infrastructure.Extensions;
using CashFlow.Infrastructure.Security.Cryptography;
using CashFlow.Infrastructure.Security.Tokens;
using CashFlow.Infrastructure.Services.LoggedUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure;
public static class DependencyInjectionExtension
{
    //Este é um método de extensão que é usado para colocar novos métodos em classes do próprio sistema e que não temos acesso ao código fonte:
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
	    services.AddScoped<IPasswordEncripter, Cryptography>();
	    services.AddScoped<ILoggedUser, LoggedUser>();
	
        AddRepositories(services);
	    AddTokenGenerator(services, configuration);

        if (configuration.IsTestEnvironment() == false)
        {
			AddDbContext(services, configuration);
		}
	}

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IExpensesReadOnlyRepository, ExpenseRepository>();
        services.AddScoped<IExpensesWriteOnlyRepository, ExpenseRepository>();
        services.AddScoped<IExpensesUpdateOnlyRepository, ExpenseRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();

	}
    private static void AddTokenGenerator(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpiresMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(config => new JwtTokenGenerator(signingKey!,expirationTimeMinutes));

    }
    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");

        var version = new Version(8, 0, 39);
        var serverVersion = new MySqlServerVersion(version);

		//Posso usar as duas linhas de código assim para pegar a versão do servidor do MySQL ou posso usar a linha abaixo
		//  var server = ServerVersion.AutoDetect(connectionString);

		services.AddDbContext<CashFlowDBContext>(config => config.UseMySql(connectionString, serverVersion));
    }
}
