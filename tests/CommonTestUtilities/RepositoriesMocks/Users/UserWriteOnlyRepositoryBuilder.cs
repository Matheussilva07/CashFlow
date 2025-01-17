using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommonTestUtilities.RepositoriesMocks.Users;
public class UserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository BuildRepository()
    {
        var mock = new Mock<IUserWriteOnlyRepository>();

        return mock.Object;
    }
}
