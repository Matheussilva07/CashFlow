using CashFlow.Application.useCases.Expenses.Reports.Excel;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.RepositoriesMocks.Expenses;
using FluentAssertions;

namespace UseCases.Tests.Expenses.Reports.Excel;
public class GenerateExpensesReportExcelUseCaseTest
{
	[Fact]
	public async Task Success()
	{
		var loggedUser = UserBuilder.Build();
		var expenses = ExpenseBuilder.Collection(loggedUser);

		var useCase = CreateUseCase(loggedUser, expenses);

		var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));

		result.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task Success_Empty()
	{
		var loggedUser = UserBuilder.Build();

		var useCase = CreateUseCase(loggedUser, new List<Expense>());

		var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));

		result.Should().BeEmpty();
	}

	private GenerateExpenseReportExcelUseCase CreateUseCase(User user, List<Expense> expenses)
	{
		var respository = new ExpensesReadOnlyRepositoryBuilder().FilterByMonth(user , expenses).Build();
		var loggedUser = LoggedUserBuilder.Build(user);

		return new GenerateExpenseReportExcelUseCase(respository, loggedUser);
	}
}
