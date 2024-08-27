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
    public void Add(Expense expense)
    {
        _dbContext.Expenses.Add(expense);
    }

    //Essa é a classe que será responsável por persistir os dados no banco de dados.
}
