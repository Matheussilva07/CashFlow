using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.useCases.Users.Profile;
public class GetUserProfileUseCase : IGetUserProfileUseCase
{
	private readonly IMapper _mapper;
	private readonly ILoggedUser _loggedUser;

	public GetUserProfileUseCase(IMapper mapper, ILoggedUser loggedUser)
	{
		_mapper = mapper;
		_loggedUser = loggedUser;
	}

	public async Task<ResponseUserProfileJson> Execute()
	{
		var user = await _loggedUser.GetUser();

		return _mapper.Map<ResponseUserProfileJson>(user);
	}
}
