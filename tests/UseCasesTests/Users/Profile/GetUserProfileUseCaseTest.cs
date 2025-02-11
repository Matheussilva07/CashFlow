﻿using CashFlow.Application.useCases.Users.Profile;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.RepositoriesMocks.Expenses;
using FluentAssertions;

namespace UseCases.Tests.Users.Profile;
public class GetUserProfileUseCaseTest
{
	[Fact]
	public async Task Success()
	{
		var user = UserBuilder.Build();
		var useCase = CreateUseCase(user);

		var result = await useCase.Execute();

		result.Should().NotBeNull();
		result.Name.Should().Be(user.Name);
		result.Email.Should().Be(user.Email);	
	}
	private GetUserProfileUseCase CreateUseCase(User user)
	{
		var mapper = MapperBuilder.Build();
		var loggedUser = LoggedUserBuilder.Build(user);

		return new GetUserProfileUseCase(mapper, loggedUser);
	}
}
