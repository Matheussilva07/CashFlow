using CashFlow.Domain.Entities;
using CashFlow.Domain.Services.LoggedUser;
using Moq;

namespace CommonTestUtilities.RepositoriesMocks.Expenses;
public class LoggedUserBuilder
{
	public static ILoggedUser Build(User user)
	{
		var mock = new Mock<ILoggedUser>();

		mock.Setup(loggedUser => loggedUser.GetUser()).ReturnsAsync(user);

		return mock.Object;	
	}
}
