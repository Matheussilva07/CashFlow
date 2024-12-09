using CashFlow.Domain.Repositories;
using Moq;

namespace CommonTestUtilities.Repositories;
public class UnitOfWorkBuilder
{
	public static IUnitOfWork BuildUnitOfWork()
	{
		var mock = new Mock<IUnitOfWork>();

		return mock.Object;
	}
}
