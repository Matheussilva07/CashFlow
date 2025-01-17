using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.Tests;

/// <summary>
/// Nesta Classe estamos configurando o banco de dados em memoria no qual testaremos nossa API
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
	private CashFlow.Domain.Entities.User _user;
	private string _password;
	private string _token;
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.UseEnvironment("FakeEnvironment").ConfigureServices(services =>
		{
			var provider = services.AddEntityFrameworkInMemoryDatabase()
			   .BuildServiceProvider();

			services.AddDbContext<CashFlowDBContext>( config =>
			{
				config.UseInMemoryDatabase("InMemoryDbForTesting");
				config.UseInternalServiceProvider(provider);
				
			});

			var scope = services.BuildServiceProvider().CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<CashFlowDBContext>();
			var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();

			StartDatabase(dbContext, passwordEncripter);

			var tokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();
			_token = tokenGenerator.GenerateToken(_user);
		});
	}

	public string GetName() => _user.Name;
	public string GetEmail() => _user.Email;
	public string GetPassword() => _password;
	public string GetToken() => _token;
	
	private void StartDatabase(CashFlowDBContext dBContext, IPasswordEncripter passwordEncripter)
	{
		_user = UserBuilder.BuildUserEntity();
		_password = _user.Password;

		_user.Password = passwordEncripter.Encrypt(_user.Password);

		dBContext.Users.Add(_user);

		dBContext.SaveChanges();
	}
}








//Outra opção --> var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();