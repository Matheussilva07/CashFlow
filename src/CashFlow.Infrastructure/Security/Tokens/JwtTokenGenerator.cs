using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CashFlow.Infrastructure.Security.Tokens;
internal class JwtTokenGenerator : IAccessTokenGenerator
{
	private readonly string _signingKey;
	private readonly uint _expirationTimeMinutes;
    public JwtTokenGenerator(string signingKey,uint expirationTimeMinutes)
    {
        _signingKey = signingKey;
        _expirationTimeMinutes = expirationTimeMinutes;
    }
    public string GenerateToken(User user)
    {
		//Claims são declarações sobre uma entidade (geralmente o usuário) e informações adicionais que são usadas em tokens de segurança,
        //como o JWT. Elas representam as informações contidas no payload do token e podem incluir dados de identificação, permissões,
        //e outras informações relevantes para autenticação e autorização.

		var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Sid, user.UserIdentifier.ToString())
        };

		//O código abaixo faz parte da criação de um token JWT e define os parâmetros que serão usados para gerar o token.

		var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
            SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature),
            Subject = new ClaimsIdentity(claims)
        };

        //A Classe abaixo é usada para gerar um criador de tokens e por meio do objeto criado por ela temos acesso ao método CreateToken.

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        //O método WriteToken faz a conversão do token para string.

        return tokenHandler.WriteToken(securityToken);
    }


	//O método abaixo é o reponsável por fazer a conversão do atributo _signingKey que é do tipo string em um tipo SecurityKey.
	private SymmetricSecurityKey SecurityKey()
    {
       var key = Encoding.UTF8.GetBytes(_signingKey);
        
        return new SymmetricSecurityKey(key);
    }



    //O tempo de expiração de um token em ambiente de desenvolvimento geralmente é maior que em linha de produção.
    //Em linha de produção o tempo é menor para ter uma segurança maior


    //Não esquecer de instalar o package nutGet Binder na infrastructure
}
