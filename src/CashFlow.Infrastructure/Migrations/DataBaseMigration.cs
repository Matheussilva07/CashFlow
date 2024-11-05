using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure.Migrations;
public static class DataBaseMigration
{
	//Esse método é o responsável por fazer a migração de forma automática
	public async static Task MigrateDatabase(IServiceProvider serviceProvider)
	{
		 var dbContext = serviceProvider.GetRequiredService<CashFlowDBContext>();

		 await dbContext.Database.MigrateAsync();
	}
}
