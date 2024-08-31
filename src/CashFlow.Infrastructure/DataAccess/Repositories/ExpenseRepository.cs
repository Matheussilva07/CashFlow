using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Infrastructure.DataAccess.Repositories;
internal class ExpenseRepository : IExpensesRepository
{

    private readonly CashFlowDBContext _dbContext;
    public ExpenseRepository(CashFlowDBContext cashFlowDBContext)
    {
        this._dbContext = cashFlowDBContext;
    }
    public async Task Add(Expense expense) => await _dbContext.Expenses.AddAsync(expense);









}
