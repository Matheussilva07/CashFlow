namespace CashFlow.Domain.Security.Cryptography;
public interface IPasswordEncripter
{
	string Encrypt(string password);
	bool PasswordMatch(string password, string passwordHash);
}
