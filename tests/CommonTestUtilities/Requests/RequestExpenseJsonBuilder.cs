using Bogus;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestExpenseJsonBuilder
{
	public static RequestExpenseJson Build()
	{
		return new Faker<RequestExpenseJson>()
			.RuleFor(e => e.Title, faker => faker.Commerce.Product())
			.RuleFor(e => e.Description, faker => faker.Commerce.ProductDescription())
			.RuleFor(e => e.Date, faker => faker.Date.Past())
			.RuleFor(e => e.PaymentType, faker => faker.PickRandom<PaymentType>())
			.RuleFor(e => e.Amount, faker => faker.Random.Decimal(1, 1000))
			.RuleFor(e => e.Tags, faker => faker.Make(2, () => faker.PickRandom<Tag>()));
	}
}
