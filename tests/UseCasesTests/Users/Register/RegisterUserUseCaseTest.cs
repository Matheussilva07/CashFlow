using CashFlow.Application.useCases.Users.Register;
using CommonTestUtilities.Requests;
using FluentAssertions;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Token;
using CommonTestUtilities.RepositoriesMocks;
using CashFlow.Exception.ExceptionsBase;
using CashFlow.Exception;

namespace UseCasesTests.Users.Register;
public class RegisterUserUseCaseTest
{
	[Fact]
	public async Task Success()
	{
		var request = RequestRegisterUserJsonBuilder.Build();
		var useCase = CreateUseCaseClasse();

		var response = await useCase.Execute(request);

		response.Should().NotBeNull();
		response.Name.Should().Be(request.Name);
		response.Token.Should().NotBeNullOrWhiteSpace();
	}

	/// <summary>
	/// O método abaixo retorna uma instância da Classe RegisterUserUseCase, pois facilitará a criação de vários testes unitários sem a necessidade
	/// de criar várias instâncias em cada teste.
	/// </summary>

	[Fact]
	public async Task Error_Name_Empty()
	{
		var request = RequestRegisterUserJsonBuilder.Build();
		request.Name = string.Empty;

		var useCase = CreateUseCaseClasse();

		var act = async () => await useCase.Execute(request);

		var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

		result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.NAME_EMPTY));
		
	}

	private RegisterUserUseCase CreateUseCaseClasse()
	{
		var mapper = MapperBuilder.BuildMapper();
		var unitOfWork = UnitOfWorkBuilder.BuildUnitOfWork();
		var writeRepository = UserWriteOnlyRepositoryBuilder.BuildRepository();
		var encrypter = PasswordEncrypterBuilder.BuildEncrypter();
		var tokenGenerator = JwtTokenGeneratorBuilder.BuildTokenGenerator();
		var readRepository = new UserReadOnlyRepositoryBuilder().BuildReadOnlyRepository();

		return new RegisterUserUseCase(mapper, unitOfWork, encrypter, readRepository, writeRepository, tokenGenerator);

	}
}
