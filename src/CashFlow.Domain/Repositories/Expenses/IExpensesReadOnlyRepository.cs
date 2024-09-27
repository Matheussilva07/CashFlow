using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;
public interface IExpensesReadOnlyRepository
{
    public Task<List<Expense>> GetAll();
    public Task<Expense?> GetById(int id);
    public Task<List<Expense>> FilterByMonth(DateOnly month);
}
