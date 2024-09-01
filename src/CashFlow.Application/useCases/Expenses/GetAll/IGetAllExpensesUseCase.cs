using CashFlow.Communication.Responses;

namespace CashFlow.Application.useCases.Expenses.GetAll;
public interface IGetAllExpensesUseCase
{
    Task<ResponseExpensesJson> Execute();
}
