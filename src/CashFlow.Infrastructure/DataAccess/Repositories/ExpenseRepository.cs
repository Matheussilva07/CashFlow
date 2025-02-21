using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CashFlow.Infrastructure.DataAccess.Repositories;
internal class ExpenseRepository : IExpensesReadOnlyRepository , IExpensesWriteOnlyRepository , IExpensesUpdateOnlyRepository
{

    private readonly CashFlowDBContext _dbContext;
    public ExpenseRepository(CashFlowDBContext cashFlowDBContext) => _dbContext = cashFlowDBContext;  

    public async Task Add(Expense expense)
    {
         await _dbContext.Expenses.AddAsync(expense);
    }

    public async Task<bool> Delete(long id, User user)
    {
        var result = await _dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == user.Id);

        if(result is null)
        {
            return false;
        }

        _dbContext.Expenses.Remove(result);

        return true;
    }

    public async Task<List<Expense>> GetAll(User user)
    {
     //Sempre que for recuperar uma informação do banco de dados e não nenhuma atualização for feita, é importante usar esse método AsNoTracking pois ele optimiza essa ação:

      return await _dbContext.Expenses.AsNoTracking().Where(expense => expense.UserId == user.Id).ToListAsync();
    }

    async Task<Expense?> IExpensesReadOnlyRepository.GetById(long id, User user)
    {
      return await GetFullExpense().AsNoTracking().FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == user.Id);
    }      

    async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(User user,long id)
    {
        //No caso de métodos para pesquisar uma despesa para então poder atualizada, não podemos usar o AsNoTracking(), já que serão feitas modificações.

       return await GetFullExpense().FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == user.Id);
    }

    public void Update(Expense expense)
    {
        _dbContext.Update(expense);
    }

	public async Task<List<Expense>> FilterByMonth(User user, DateOnly date)
	{
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1 ).Date;

        var daysInMonth = DateTime.DaysInMonth(year: date.Year,month: date.Month);

        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59).Date;


		return await _dbContext.Expenses
            .AsNoTracking()
            .Where(expense => expense.UserId == user.Id && expense.Date >= startDate && expense.Date <= endDate)
            .OrderBy(ex => ex.Date)
            .ToListAsync();
	}

    private IIncludableQueryable<Expense, ICollection<Tag>> GetFullExpense()
    {
        return _dbContext.Expenses.Include(expense => expense.Tags);

	}
}
