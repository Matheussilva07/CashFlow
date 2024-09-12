using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;
internal class ExpenseRepository : IExpensesReadOnlyRepository , IExpensesWriteOnlyRepository
{

    private readonly CashFlowDBContext _dbContext;
    public ExpenseRepository(CashFlowDBContext cashFlowDBContext)
    {
       _dbContext = cashFlowDBContext;
    }
    public async Task Add(Expense expense)
    {
         await _dbContext.Expenses.AddAsync(expense);
    }

    public async Task<List<Expense>> GetAll()
    {
     //Sempre que for recuperar uma informação do banco de dados e não nenhuma atualização for feita, é importante usar esse método AsNoTracking pois ele optimiza essa ação:

      return await _dbContext.Expenses.AsNoTracking().ToListAsync();
    }

    public async Task<Expense?> GetById(int id)
    {
      return await _dbContext.Expenses.AsNoTracking().FirstOrDefaultAsync(expense => expense.Id == id);
    }
}
