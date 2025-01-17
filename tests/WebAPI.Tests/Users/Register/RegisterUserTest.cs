using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WebAPI.Tests.InlineData;

namespace WebAPI.Tests.Users.Register;

public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    private const string COMPLEMENT_URL = "api/user";

	public RegisterUserTest(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
    }

    [Fact]
	public async Task Success()
	{
        var request = RequestRegisterUserJsonBuilder.Build();

        var result = await _httpClient.PostAsJsonAsync(COMPLEMENT_URL, request);

        result.StatusCode.Should().Be(HttpStatusCode.Created);      

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
        response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
	}

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Name(string cultureInfo)
    {
		var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(cultureInfo));

		var result = await _httpClient.PostAsJsonAsync(COMPLEMENT_URL, request);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

       var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray(); //O método GetProperty() pegará o objeto Lista de erros na Classe ResponseErrorJson e por ser uma lista, precisamos usar o outro método EnumerateArray() para retornar a lista de erros;

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(cultureInfo));

		errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
	}
}


//No caso de testes de integração, que são usados para verificar todo o fluxo da API, desde o Endpoint,
//regras de negócios, repositórios até o banco de dados, nós precisamos implementar a interface IClassFixture na Classe onde faremos 
//os testes e essa interface recebe entre maior e menor a classe WebApplicationFactory que tem o papel de simular um servidor fictício 
//onde nossa API será executada e entre menor e maior precisamos indicar qual o ponto de entrada e este é no arquivo Program.
//Logo, precisamos passar uma classe, no entanto, no arquivo Program não há uma, para isso para criarmos uma classe publica e pracial com o nome Program
//e passar ele na classe WebApplicationFactory;