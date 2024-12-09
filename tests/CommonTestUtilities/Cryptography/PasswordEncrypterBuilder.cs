using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Cryptography;
public class PasswordEncrypterBuilder
{
	public static IPasswordEncripter BuildEncrypter()
	{
		var mock = new Mock<IPasswordEncripter>();

		mock.Setup(passwordEncryptor => passwordEncryptor.Encrypt(It.IsAny<string>())).Returns("mwdliqwjdoqiwjdçndnç");

		return mock.Object;
	}
}
