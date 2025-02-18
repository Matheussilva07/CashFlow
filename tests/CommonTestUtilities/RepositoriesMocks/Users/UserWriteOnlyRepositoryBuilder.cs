using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommonTestUtilities.RepositoriesMocks.Users;
public class UserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Build()
    {
        var mock = new Mock<IUserWriteOnlyRepository>();

        return mock.Object;
    }
}
