using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using Moq;

namespace CommonTestUtilities.Token;
public class JwtTokenGeneratorBuilder
{
	public static IAccessTokenGenerator BuildTokenGenerator()
	{
		var mock = new Mock<IAccessTokenGenerator>();

		mock.Setup(accessTokenGenerator => accessTokenGenerator.GenerateToken(It.IsAny<User>())).Returns("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c");

		return mock.Object;
	}
}

//Após criamos o mock da interface na linha 10, precisamos definir qual o tipo de retorno esse mock terá.
//Para definir o retorno, usamos o método Setup que já vem do pacote nuGet moq, nele fazemos uma função lambda que através dela acessamos
//o método implementado na interface 'IAccessTokenGenerator'. Como esse método tem um parâmentro, usando a Classe It que tem o método IsAny 
//este método dirá ao método GenerateToken que ele pode funcionar normalmente sem o parâmentro, está função precisa receber entre <> o tipo do parâmentro
// depois basta chamar o método Returns e colocar algum valor que corresponda ao tipo do método para ser retornado quando o mock for acionado;
