namespace CashFlow.Application.useCases.Expenses.Delete;
public interface IDeleteExpenseUseCase
{
    Task Execute(int id);
}
