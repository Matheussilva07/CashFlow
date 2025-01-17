using CashFlow.Communication.Requests;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Net.Http.Json;
using System.Text.Json;
using WebAPI.Tests.InlineData;
using System.Net.Http.Headers;
using CashFlow.Exception;
using System.Globalization;
using DocumentFormat.OpenXml.Spreadsheet;

namespace WebAPI.Tests.Login.DoLogin;

public class DoLoginTest : IClassFixture<CustomWebApplicationFactory>
{
	private const string METHOD = "api/Login";

    private readonly HttpClient _httpClient;

    private readonly string _email;
    private readonly string _name;
    private readonly string _password;

    public DoLoginTest(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
        _email = webApplicationFactory.GetEmail();
        _name = webApplicationFactory.GetName();
        _password = webApplicationFactory.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson 
        {
            Email = _email,
            Password = _password
        };

        var response = await _httpClient.PostAsJsonAsync(METHOD, request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().Should().Be(_name);
        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();

	}

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Login_Invalid(string culture)
    {

        var request = RequestLoginJsonBuilder.Build();

        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));

        var response = await _httpClient.PostAsJsonAsync(METHOD, request);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals(expectedMessage));
    }
}

