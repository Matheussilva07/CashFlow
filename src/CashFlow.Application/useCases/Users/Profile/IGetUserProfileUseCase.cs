using CashFlow.Communication.Responses;

namespace CashFlow.Application.useCases.Users.Profile;
public interface IGetUserProfileUseCase
{
	Task<ResponseUserProfileJson> Execute();
}
