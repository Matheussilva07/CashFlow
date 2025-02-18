﻿using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.useCases.Users.Delete;
public class DeleteUserAccountUseCase : IDeleteUserAccountUseCase
{
	private readonly ILoggedUser _loggedUser;
	private readonly IUserWriteOnlyRepository _repository;
	private readonly IUnitOfWork _unitOfWork;

	public DeleteUserAccountUseCase(ILoggedUser loggedUser, IUserWriteOnlyRepository repository, IUnitOfWork unitOfWork)
	{
		_loggedUser = loggedUser;
		_repository = repository;
		_unitOfWork = unitOfWork;
	}

	public async Task Execute()
	{
		var user = await _loggedUser.GetUser();

	    await _repository.Delete(user);

		await _unitOfWork.Commit();
	}
}
