using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;
internal class ExpenseRepository : IExpensesReadOnlyRepository , IExpensesWriteOnlyRepository , IExpensesUpdateOnlyRepository
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
    public async Task<bool> Delete(int id)
    {
        var result = await _dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);

        if(result is null)
        {
            return false;
        }

        _dbContext.Expenses.Remove(result);

        return true;
    }
    public async Task<List<Expense>> GetAll()
    {
     //Sempre que for recuperar uma informação do banco de dados e não nenhuma atualização for feita, é importante usar esse método AsNoTracking pois ele optimiza essa ação:

      return await _dbContext.Expenses.AsNoTracking().ToListAsync();
    }
    async Task<Expense?> IExpensesReadOnlyRepository.GetById(int id)
    {
      return await _dbContext.Expenses.AsNoTracking().FirstOrDefaultAsync(expense => expense.Id == id);
    }
      
    async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(int id)
    {
        //No caso de métodos para pesquisar uma despesa para então poder atualizada, não podemos usar o AsNoTracking()

       return await _dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);
    }
    public void Update(Expense expense)
    {
        _dbContext.Update(expense);
    }

	public async Task<List<Expense>> FilterByMonth(DateOnly date)
	{
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1 ).Date;

        var daysInMonth = DateTime.DaysInMonth(year: date.Year,month: date.Month);

        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59).Date;


		return await _dbContext.Expenses
            .AsNoTracking()
            .Where(expense => expense.Date >= startDate && expense.Date <= endDate)
            .OrderBy(ex => ex.Date)
            .ToListAsync();
	}
}
