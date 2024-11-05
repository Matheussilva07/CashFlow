namespace CashFlow.Domain.Repositories.Users;
public interface IUserReadOnlyRepository
{
	//Método para verificar se já existe um usuário com este email.
	Task<bool> ExistActiveUserWithEmail(string email);
}
