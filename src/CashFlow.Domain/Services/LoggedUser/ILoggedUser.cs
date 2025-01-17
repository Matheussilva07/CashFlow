using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Services.LoggedUser;
public interface ILoggedUser
{
	/// <summary>
	/// Este método é resonsável por pegar o identificador do usuário que está logado para poder registrar as despesas com seu Id;
	/// </summary>
	/// <returns>
	/// Retorna uma entidade User
	/// </returns>
	Task<User> GetUser();
}
