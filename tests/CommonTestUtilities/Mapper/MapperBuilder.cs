using AutoMapper;
using CashFlow.Application.AutoMapper;

namespace CommonTestUtilities.Mapper;
public class MapperBuilder
{
	public static IMapper BuildMapper()
	{
		var mapper = new MapperConfiguration(config =>
		{
			config.AddProfile(new AutoMapping());
		});

		return mapper.CreateMapper();
	}
}

//Esse método é o responsável por criar o Imapper que é necessário lá no construtor da instância de RegisterUserUseCaseTest,
//pois essa classe concreta necessita desse parâmetro;