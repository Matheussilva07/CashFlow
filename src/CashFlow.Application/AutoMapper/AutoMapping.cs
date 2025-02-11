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
        //A primeira classe é a origem dos dados e a segunda o destino dos dados:

        CreateMap<RequestExpenseJson, Expense>();

        //O código abaixo serve para definir o automapper,
        //como também para definir que a propriedade Password deve ser ignorada no mapeamento de propriedades

        CreateMap<RequestRegisterUserJson, User>().ForMember(user => user.Password, configuracao => configuracao.Ignore());

    }
    private void EntityToResponse()
    {
        CreateMap<Expense, ResponseRegisteredExpenseJson>();
        CreateMap<Expense, ResponseShortExpensesJson>();
        CreateMap<Expense, ResponseExpenseJson>();

        CreateMap<User, ResponseUserProfileJson>();

    }
}
