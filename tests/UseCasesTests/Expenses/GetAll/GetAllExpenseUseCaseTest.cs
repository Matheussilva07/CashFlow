﻿using CashFlow.Application.useCases.Expenses.GetAll;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.RepositoriesMocks.Expenses;
using FluentAssertions;

namespace UseCases.Tests.Expenses.GetAll;
public class GetAllExpenseUseCaseTest
{
	[Fact]
	public async Task Success()
	{
		var loggedUser = UserBuilder.Build();
		var expenses = ExpenseBuilder.Collection(loggedUser,5);

		var useCase = CreateUseCase(loggedUser, expenses);

		var result = await useCase.Execute();

		result.Should().NotBeNull();
		result.Expenses.Should().NotBeNullOrEmpty().And.AllSatisfy(expense =>
		{
			expense.Id.Should().BeGreaterThan(0);
			expense.Title.Should().NotBeNullOrEmpty();
			expense.Amount.Should().BeGreaterThan(0);
		});
	}

	private GetAllExpensesUseCase CreateUseCase(User user, List<Expense> expenses)
	{
		var repository = new ExpensesReadOnlyRepositoryBuilder().GetAll(user, expenses).Build();
		var mapper = MapperBuilder.Build();
		var loggedUser = LoggedUserBuilder.Build(user);

		return new GetAllExpensesUseCase(repository: repository, mapper: mapper, loggedUser: loggedUser);
	}
}
