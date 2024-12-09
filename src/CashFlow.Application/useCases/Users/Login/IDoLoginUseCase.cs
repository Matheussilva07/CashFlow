using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.useCases.Users.Login;
public interface IDoLoginUseCase
{
	Task<ResponseRegisteredUserJson> Execute(RequestLoginJson requestLoginJson);
}
