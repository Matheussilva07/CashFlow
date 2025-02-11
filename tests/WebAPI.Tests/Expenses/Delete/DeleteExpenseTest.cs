using FluentAssertions;
using System.Net;

namespace WebAPI.Tests.Expenses.Delete;

public class DeleteExpenseTest : CashFLowClassFixture
{
	private const string METHOD = "api/Expenses";

	private readonly string _token;
	private readonly long _expenseId;

	public DeleteExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
	{
		_token = webApplicationFactory.User_Team_Member.GetToken();
		_expenseId = webApplicationFactory.Expense_MemberTeam.GetById();
	}

	[Fact]
	public async Task Success()
	{
		var result = await DoDelete(requestUri: $"{METHOD}/{_expenseId}",token: _token);

		result.StatusCode.Should().Be(HttpStatusCode.NoContent);

		result = await DoGet( requestUri: $"{METHOD}/{_expenseId}",token: _token);

		result.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}
}
