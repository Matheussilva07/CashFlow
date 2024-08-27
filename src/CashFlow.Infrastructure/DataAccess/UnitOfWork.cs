using CashFlow.Domain.Repositories;

namespace CashFlow.Infrastructure.DataAccess;
internal class UnitOfWork : IUnitOfWork
{
    private readonly CashFlowDBContext _dbContext;
    public UnitOfWork(CashFlowDBContext dbContext)
    {
        this._dbContext = dbContext;
    }
    public void Commit()
    {
     _dbContext.SaveChanges();
    }
}
