﻿using CashFlow.Application.useCases.Users.ChangePassword;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.RepositoriesMocks.Expenses;
using CommonTestUtilities.RepositoriesMocks.Users;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Tests.Users.ChangePassword;
public class ChangePasswordUseCaseTest
{
	[Fact]
	public async Task Success()
	{
		var user = UserBuilder.Build();

		var request = RequestChangePasswordJsonBuilder.Build();

		var useCase = CreateUseCase(user, request.Password);

		var act = async () => await useCase.Execute(request);

		await act.Should().NotThrowAsync();
	}

	[Fact]
	public async Task Error_NewPassword_Empty()
	{
		var user = UserBuilder.Build();

		var request = RequestChangePasswordJsonBuilder.Build();
		request.NewPassword = string.Empty;

		var useCase = CreateUseCase(user, request.Password);

		var act = async () => { await useCase.Execute(request); };

		var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

		result.Where(e => e.GetErrors().Count == 1 && e.GetErrors().Contains(ResourceErrorMessages.INVALID_PASSWORD));
	}

	[Fact]
	public async Task Error_CurrentPassword_Different()
	{
		var user = UserBuilder.Build();

		var request = RequestChangePasswordJsonBuilder.Build();

		var useCase = CreateUseCase(user);

		var act = async () => { await useCase.Execute(request); };

		var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

		result.Where(e => e.GetErrors().Count == 1 && e.GetErrors().Contains(ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
	}

	private ChangePasswordUseCase CreateUseCase(User user, string? password = null)
	{
		var unitOfWork = UnitOfWorkBuilder.Build();
		var userUpdateRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
		var loggedUser = LoggedUserBuilder.Build(user);
		var passwordEncryptor = new PasswordEncrypterBuilder().Verify(password).Build();		
		
		return new ChangePasswordUseCase(loggedUser,userUpdateRepository,unitOfWork, passwordEncryptor);
	}
}
