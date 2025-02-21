using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Tests.Resources;

namespace WebAPI.Tests;

/// <summary>
/// Nesta Classe estamos configurando o banco de dados em memoria no qual testaremos nossa API
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
	public ExpenseIdentityManager Expense_Admin { get; private set; } = default!;
	public ExpenseIdentityManager Expense_MemberTeam { get; private set; } = default!;
	public UserIdentityMananger User_Team_Member { get; private set; } = default!;
	public UserIdentityMananger User_Admin { get; private set; } = default!;

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.UseEnvironment("FakeEnvironment").ConfigureServices(services =>
		{
			var provider = services.AddEntityFrameworkInMemoryDatabase()
			   .BuildServiceProvider();

			services.AddDbContext<CashFlowDBContext>(config =>
			{
				config.UseInMemoryDatabase("InMemoryDbForTesting");
				config.UseInternalServiceProvider(provider);

			});

			var scope = services.BuildServiceProvider().CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<CashFlowDBContext>();
			var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();
			var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

			StartDatabase(dbContext, passwordEncripter, accessTokenGenerator);

		});
	}

	public long GetExpenseId() => Expense_MemberTeam.GetById();

	private void StartDatabase(CashFlowDBContext dBContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator)
	{
		var userTeamMember = AddUserTeamMember(dBContext, passwordEncripter, accessTokenGenerator);
		var expenseTeamMember = AddExpenses(dBContext, userTeamMember, expenseId: 1, tagId: 1);
		Expense_MemberTeam = new ExpenseIdentityManager(expenseTeamMember);

		var userAdmin = AddUserAdmin(dBContext, passwordEncripter, accessTokenGenerator);
		var expenseAdmin = AddExpenses(dBContext, userAdmin, expenseId: 2, tagId: 2);
		Expense_Admin = new ExpenseIdentityManager(expenseAdmin);


		dBContext.SaveChanges();
	}

	private User AddUserTeamMember(CashFlowDBContext dBContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator)
	{
		var user = UserBuilder.Build();
		user.Id = 1;

		var password = user.Password;

		user.Password = passwordEncripter.Encrypt(user.Password);

		dBContext.Users.Add(user);

		var token = accessTokenGenerator.GenerateToken(user);

		User_Team_Member = new UserIdentityMananger(user, password, token);

		return user;
	}

	private User AddUserAdmin(CashFlowDBContext dBContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator)
	{
		var user = UserBuilder.Build(Roles.ADMIN);
		user.Id = 2;

		var password = user.Password;

		user.Password = passwordEncripter.Encrypt(user.Password);

		dBContext.Users.Add(user);

		var token = accessTokenGenerator.GenerateToken(user);

		User_Admin = new UserIdentityMananger(user, password, token);

		return user;
	}

	private Expense AddExpenses(CashFlowDBContext dBContext, User user, long expenseId, long tagId)
	{
		var expense = ExpenseBuilder.Build(user);
		expense.Id = expenseId;

		foreach (var tag in expense.Tags)
		{
			tag.Id = tagId;
			tag.ExpenseId = expenseId;
		}

		dBContext.Expenses.Add(expense);

		return expense;
	}
}








//Outra opção --> var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();