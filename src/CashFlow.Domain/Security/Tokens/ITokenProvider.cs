namespace CashFlow.Domain.Security.Tokens;
public interface ITokenProvider
{
	/// <summary>
	/// Este método é o responsável por pegar o token da requisição que será usado na Classe LoggedUser em Cashflow.Infrastructure
	/// </summary>
	/// <returns>Retorna o token da requisição</returns>
	string TakeTokenFromRequest();
}
