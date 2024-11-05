using BCrypt.Net;
using BC = BCrypt.Net.BCrypt;
using CashFlow.Domain.Security.Cryptography;

namespace CashFlow.Infrastructure.Security;
public class Cryptography : IPasswordEncripter
{
	public string Encrypt(string password)
	{
		string passwordHash = BC.HashPassword(password);

		return passwordHash;
	}
}
