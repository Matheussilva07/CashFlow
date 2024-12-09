using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;
internal class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository
{
    private readonly CashFlowDBContext _dbContext;
    public UserRepository(CashFlowDBContext dBContext)
	{
		_dbContext = dBContext;
	}

	public async Task Add(User user)
	{
		await _dbContext.Users.AddAsync(user);
	}
	public async Task<bool> ExistActiveUserWithEmail(string email)
	{
		return await _dbContext.Users.AnyAsync(user => user.Email.Equals(email));
	}

	public async Task<User?> GetUserByEmail(string email)
	{
		return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
	}
}
