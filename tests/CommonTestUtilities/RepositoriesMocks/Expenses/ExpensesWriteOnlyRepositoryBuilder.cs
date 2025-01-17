using CashFlow.Domain.Repositories.Expenses;
using Moq;

namespace CommonTestUtilities.RepositoriesMocks.Expenses;
public class ExpensesWriteOnlyRepositoryBuilder
{
	public static IExpensesWriteOnlyRepository BuildMock()
	{
		var mock = new Mock<IExpensesWriteOnlyRepository>();

		return mock.Object;
	}
}
