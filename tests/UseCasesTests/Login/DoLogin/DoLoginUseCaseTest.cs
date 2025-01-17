﻿using CashFlow.Application.useCases.Users.Login;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.RepositoriesMocks;
using CommonTestUtilities.RepositoriesMocks.Users;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;

namespace UseCases.Tests.Login.DoLogin;
public class DoLoginUseCaseTest
{
	[Fact]
	public async Task Success()
	{
		var user = UserBuilder.BuildUserEntity();

		var request = RequestLoginJsonBuilder.Build();

		var useCase = CreateUseCase(user, request.Password);
		request.Email = user.Email;

		var result = await useCase.Execute(request);

		result.Should().NotBeNull();
		result.Name.Should().Be(user.Name);
		result.Token.Should().NotBeNullOrWhiteSpace();
	}

	[Fact]
	public async Task Error_User_Not_Found()
	{
		var user = UserBuilder.BuildUserEntity();

		var request = RequestLoginJsonBuilder.Build();

		var useCase = CreateUseCase(user, request.Password);

		var act = async () => await useCase.Execute(request);

		var result = await act.Should().ThrowAsync<InvalidLoginException>();

		result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID));
	}

	[Fact]
	public async Task Error_Passoword_Not_Match()
	{
		var user = UserBuilder.BuildUserEntity();

		var request = RequestLoginJsonBuilder.Build();
		request.Email = user.Email;

		var useCase = CreateUseCase(user);

		var act = async () => await useCase.Execute(request);

		var result = await act.Should().ThrowAsync<InvalidLoginException>();

		result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID));
	}

	private DoLoginUseCase CreateUseCase(User user, string? password = null)
	{		
		var passwordEncrypter = new PasswordEncrypterBuilder().Verify(password).Build();
		var tokenGenerator = JwtTokenGeneratorBuilder.BuildTokenGenerator();
		var readRepository = new UserReadOnlyRepositoryBuilder().GetUserByEmail(user).BuildReadOnlyRepository();

		return new DoLoginUseCase(readRepository, passwordEncrypter, tokenGenerator);
	}
}
