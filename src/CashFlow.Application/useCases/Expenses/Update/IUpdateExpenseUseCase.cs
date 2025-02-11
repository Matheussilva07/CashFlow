using CashFlow.Communication.Requests;

namespace CashFlow.Application.useCases.Expenses.Update;
public interface IUpdateExpenseUseCase
{
    Task Execute(long id, RequestExpenseJson request );
}
