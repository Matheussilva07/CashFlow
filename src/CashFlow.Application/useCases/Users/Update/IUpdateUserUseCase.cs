using CashFlow.Communication.Requests;

namespace CashFlow.Application.useCases.Users.Update;
public interface IUpdateUserUseCase
{
	Task Execute(RequestUpdateUserJson request);
}
