﻿using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;
internal class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
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

	public async Task<User> GetById(long id)
	{
		return await _dbContext.Users.FirstAsync(user => user.Id == id);
	}

	public async Task<User?> GetUserByEmail(string email)
	{
		return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
	}

	public void Update(User user)
	{
		_dbContext.Users.Update(user);
	}
}
