using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommonTestUtilities.RepositoriesMocks;
public class UserReadOnlyRepositoryBuilder
{
	private readonly Mock<IUserReadOnlyRepository> _readOnlyRepository;

	public UserReadOnlyRepositoryBuilder()
	{
		_readOnlyRepository = new Mock<IUserReadOnlyRepository>();
	}
	public IUserReadOnlyRepository BuildReadOnlyRepository() => _readOnlyRepository.Object;
}
