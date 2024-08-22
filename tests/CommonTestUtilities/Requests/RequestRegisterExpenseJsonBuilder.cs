using Bogus;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestRegisterExpenseJsonBuilder
{
    public static RequestRegisterExpenseJson Build()
    {
        var faker = new Faker();

        var request = new RequestRegisterExpenseJson()
        {
            Title = faker.Commerce.Product(),
            Description = faker.Commerce.ProductDescription(),
            Date = faker.Date.Past(),
            PaymentType = faker.PickRandom<PaymentType>(),
            Amount = faker.Random.Decimal(min: -5, max: 50)
        };

        return request;
    }
}
