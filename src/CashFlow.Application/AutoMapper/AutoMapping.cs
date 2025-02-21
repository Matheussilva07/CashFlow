using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;

namespace CashFlow.Application.AutoMapper;
public class AutoMapping : Profile
{
	public AutoMapping()
	{
		RequestToEntity();
		EntityToResponse();
	}
	private void RequestToEntity()
	{
		CreateMap<RequestRegisterUserJson, User>().ForMember(user => user.Password, configuracao => configuracao.Ignore());

		CreateMap<RequestExpenseJson, Expense>().ForMember(dest => dest.Tags, config => config.MapFrom(source => source.Tags.Distinct()));

		CreateMap<Communication.Enums.Tag, Tag>().ForMember(dest => dest.Value, config => config.MapFrom(source => source));

	}
	private void EntityToResponse()
	{
		CreateMap<Expense, ResponseExpenseJson>()
			.ForMember(destino => destino.Tags, config => config.MapFrom(origem => origem.Tags.Select(entidadeTag => entidadeTag.Value)));



		CreateMap<Expense, ResponseRegisteredExpenseJson>();
		CreateMap<Expense, ResponseShortExpensesJson>();
		CreateMap<User, ResponseUserProfileJson>();

	}
}
