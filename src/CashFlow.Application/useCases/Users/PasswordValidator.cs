using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation;
using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace CashFlow.Application.useCases.Users;
public class PasswordValidator<T> : PropertyValidator<T, string>
{
	private const string ERROR_MESSAGE_KEY = "ErrorMessage";
	public override string Name => "PasswordValidator";
	protected override string GetDefaultMessageTemplate(string errorCode)
	{
		//Este método é o responsável por pegar as mensagens fornecidas no metodo AppendArgument e lancá-las
		//e ele pega essas mensagens através das chaves que passamos como parâmetro e elas precisam estar entre {};
		
		return $"{{{ERROR_MESSAGE_KEY}}}";
	}
	public override bool IsValid(ValidationContext<T> context, string password)
	{
		if (string.IsNullOrWhiteSpace(password))
		{
			//O código abaixo lança o erro e esse método é o responsável.
			//Ele também precisa receber dois parâmetros, o primeiro é uma chave que foi definida como uma constante
			//e o segundo é a mensagem que será exibida.

			context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceErrorMessages.INVALID_PASSWORD);
			return false;
		}

		//A verificação do tamanho deve vir após a verificação dos espaços em brancos ou nulos
		//pois como ele vai verificar o Length de algo em branco? Isso geraria uma exceção.

		if (password.Length < 8)
		{
			context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceErrorMessages.INVALID_PASSWORD);
			return false;
		}
		if (Regex.IsMatch(password, @"[A-Z]+" ) == false)
		{
			context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceErrorMessages.INVALID_PASSWORD);
			return false;
		}
		if (Regex.IsMatch(password, @"[a-z]+") == false)
		{
			context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceErrorMessages.INVALID_PASSWORD);
			return false;
		}
		if (Regex.IsMatch(password, @"[0-9]+") == false)
		{
			context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceErrorMessages.INVALID_PASSWORD);
			return false;
		}
		if (Regex.IsMatch(password, @"[\!\*\.]+") == false)
		{
			context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceErrorMessages.INVALID_PASSWORD);
			return false;
		}

		return true;
	//No caso de validação de senhas, precisamos criar várias verificações, pois as senhas não tem um padrão definido, como um email, por exemplo.
	}

}
