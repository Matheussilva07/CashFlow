﻿using CashFlow.Application.useCases.Users.Delete;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.RepositoriesMocks.Expenses;
using CommonTestUtilities.RepositoriesMocks.Users;
using FluentAssertions;

namespace UseCases.Tests.Users.Delete;
public class DeleteUserAccountUseCaseTest
{
	[Fact]
	public async Task Success()
	{
		var user = UserBuilder.Build();

		var useCase = CreateUseCase(user);

		var act = async () => await useCase.Execute();

		await act.Should().NotThrowAsync();
	}

	private DeleteUserAccountUseCase CreateUseCase(User user)
	{
		var repository = UserWriteOnlyRepositoryBuilder.Build();
		var loggedUser = LoggedUserBuilder.Build(user);
		var unitOfWork = UnitOfWorkBuilder.Build();

		return new DeleteUserAccountUseCase(loggedUser, repository, unitOfWork);
	}
}
