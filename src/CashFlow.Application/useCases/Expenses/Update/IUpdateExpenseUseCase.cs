using CashFlow.Communication.Requests;

namespace CashFlow.Application.useCases.Expenses.Update;
public interface IUpdateExpenseUseCase
{
    Task Execute(int id, RequestExpenseJson request );
}
