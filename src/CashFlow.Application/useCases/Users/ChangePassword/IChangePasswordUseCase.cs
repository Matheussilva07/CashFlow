using CashFlow.Communication.Requests;

namespace CashFlow.Application.useCases.Users.ChangePassword;
public interface IChangePasswordUseCase
{
    Task Execute(RequestChangePasswordJson request);
}
