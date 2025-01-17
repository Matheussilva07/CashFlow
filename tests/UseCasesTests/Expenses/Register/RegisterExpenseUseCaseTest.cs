using AutoMapper.Execution;
using CashFlow.Application.useCases.Expenses;
using CashFlow.Application.useCases.Expenses.Register;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.RepositoriesMocks.Expenses;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Tests.Expenses.Register;
public class RegisterExpenseUseCaseTest
{
	[Fact]
	public async Task Success()
	{
		var loggedUser = UserBuilder.BuildUserEntity();
		var request = RequestRegisterExpenseJsonBuilder.Build();
		var useCase = CreateUseCase(loggedUser); 

		var result = await useCase.Execute(request);
		
		result.Should().NotBeNull();
		result.Title.Should().Be(request.Title);
	}
	[Fact]
	public async Task Error_Title_Empty()
	{
		var loggedUser = UserBuilder.BuildUserEntity();

		var request = RequestRegisterExpenseJsonBuilder.Build();
		request.Title = string.Empty;

		var useCase = CreateUseCase(loggedUser);

		var act = async () => await useCase.Execute(request);

		var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

		result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.TITLE_RIQUIRED));
	}
	private RegisterExpenseUseCase CreateUseCase(User user)
	{
		var mapper = MapperBuilder.BuildMapper();
		var unitOfWork = UnitOfWorkBuilder.BuildUnitOfWork();
		var repository = ExpensesWriteOnlyRepositoryBuilder.BuildMock();
		var loggedUser = LoggedUserBuilder.Build(user);

		return new RegisterExpenseUseCase(repository,unitOfWork,mapper,loggedUser);
	}
}
