using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebAPI.Tests.InlineData;

namespace WebAPI.Tests.Users.Register;

public class RegisterUserTest : CashFLowClassFixture
{
	private const string METHOD = "api/user";

	public RegisterUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
	{

	}

	[Fact]
	public async Task Success()
	{
		var request = RequestRegisterUserJsonBuilder.Build();

		var result = await DoPost(METHOD, request);

		result.StatusCode.Should().Be(HttpStatusCode.Created);

		var body = await result.Content.ReadAsStreamAsync();

		var response = await JsonDocument.ParseAsync(body);

		response.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
		response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
	}

	[Theory]
	[ClassData(typeof(CultureInlineDataTest))]
	public async Task Error_Empty_Name(string culture)
	{
		var request = RequestRegisterUserJsonBuilder.Build();
		request.Name = string.Empty;

		var result = await DoPost(requestUri: METHOD, request: request, culture: culture);

		result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

		var body = await result.Content.ReadAsStreamAsync();

		var response = await JsonDocument.ParseAsync(body);

		var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray(); //O método GetProperty() pegará o objeto Lista de erros na Classe ResponseErrorJson e por ser uma lista, precisamos usar o outro método EnumerateArray() para retornar a lista de erros;

		var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

		errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
	}
}


//No caso de testes de integração, que são usados para verificar todo o fluxo da API, desde o Endpoint,
//regras de negócios, repositórios até o banco de dados, nós precisamos implementar a interface IClassFixture na Classe onde faremos 
//os testes e essa interface recebe entre maior e menor a classe WebApplicationFactory que tem o papel de simular um servidor fictício 
//onde nossa API será executada e entre menor e maior precisamos indicar qual o ponto de entrada e este é no arquivo Program.
//Logo, precisamos passar uma classe, no entanto, no arquivo Program não há uma, para isso para criarmos uma classe publica e pracial com o nome Program
//e passar ele na classe WebApplicationFactory;