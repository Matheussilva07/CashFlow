using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;
internal class ExpenseRepository : IExpensesRepository
{

    private readonly CashFlowDBContext _dbContext;
    public ExpenseRepository(CashFlowDBContext cashFlowDBContext)
    {
       _dbContext = cashFlowDBContext;
    }
    public async Task Add(Expense expense) => await _dbContext.Expenses.AddAsync(expense);

    public async Task<List<Expense>> GettAll()
    {
      return await _dbContext.Expenses.ToListAsync();
    }
}
