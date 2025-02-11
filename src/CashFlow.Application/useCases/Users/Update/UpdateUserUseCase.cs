using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using DocumentFormat.OpenXml.Office2016.Excel;
using FluentValidation;
using FluentValidation.Results;
using System.ComponentModel.DataAnnotations;

namespace CashFlow.Application.useCases.Users.Update;
public class UpdateUserUseCase : IUpdateUserUseCase
{
	private readonly ILoggedUser _loggedUser;
	private readonly IUserUpdateOnlyRepository _updateOnlyRepository;
	private readonly IUserReadOnlyRepository _readOnlyRepository;
	private readonly IUnitOfWork _unitOfWork;

	public UpdateUserUseCase(ILoggedUser loggedUser, IUserReadOnlyRepository readOnlyRepository, IUnitOfWork unitOfWork, IUserUpdateOnlyRepository userUpdateOnlyRepository)
	{
		_loggedUser = loggedUser;
		_readOnlyRepository = readOnlyRepository;
		_unitOfWork = unitOfWork;
		_updateOnlyRepository = userUpdateOnlyRepository;
	}

	public async Task Execute(RequestUpdateUserJson request)
	{
		var loggedUser = await _loggedUser.GetUser();
		
		Validate(request, loggedUser.Email);

		var user = await _updateOnlyRepository.GetById(loggedUser.Id);

		user.Name = request.Name;
		user.Email = request.Email;

		_updateOnlyRepository.Update(user);

		await _unitOfWork.Commit();
	}

	private async void Validate(RequestUpdateUserJson request, string currentEmail)
	{
		var validator = new UpdateUserValidator();

		var result = validator.Validate(request);

		if (currentEmail.Equals(request.Email) == false)
		{
			bool userExist = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);

			if (userExist)
				result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
		}

		if(result.IsValid == false)
		{
			var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

			throw new ErrorOnValidationException(errorMessages);
		}
	}
}
