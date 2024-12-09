using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure.Migrations;
public static class DataBaseMigration
{
	//Esse método é o responsável por fazer a migração de forma automática.
	//Ver a implementação de outro método na Classe Program que atua em conjunto com este.
	public async static Task MigrateDatabase(IServiceProvider serviceProvider)
	{
		 var dbContext = serviceProvider.GetRequiredService<CashFlowDBContext>();

		 await dbContext.Database.MigrateAsync();
	}
}
