using CashFlow.API.Filters;
using CashFlow.API.Middleware;
using CashFlow.Application;
using CashFlow.Infrastructure;
using CashFlow.Infrastructure.Migrations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using CashFlow.Infrastructure.Extensions;
using CashFlow.Domain.Security.Tokens;
using CashFlow.API.Token;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(config =>
{
	config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Description = @"JWT Authorization header using the Bearer scheme.
						Enter 'Bearer' [space] and then your token in the text input below.	
						Exemplo: 'Bearer 123diwqpodi'",
		In = ParameterLocation.Header,
		Scheme = "Bearer",
		Type = SecuritySchemeType.ApiKey

	});

	config.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				},
				Scheme = "oauth2",
				Name ="Bearer",
				In = ParameterLocation.Header
			},
			new List<string>()
		}
	});
});

//O código abaixo é para definir o filtro das exceções que fizemos:
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));


//O código abaixo serve para criar as injeções de dependências em uma classe propria por meio de metodos de extensão:
builder.Services.AddInfrastructure(builder.Configuration);
//O código abaixo serve para criar as injeções de dependências em uma classe propria por meio de metodos de extensão:
builder.Services.AddApplication();

builder.Services.AddRouting(option => option.LowercaseUrls = true);
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();
builder.Services.AddHttpContextAccessor();

//O código abaixo foi inserido para configurar o Swagger:

builder.Services.AddSwaggerGen(options => {
	options.MapType<DateOnly>(() => new OpenApiSchema
	{
		Type = "string",
		Format = "date"
	});
});

//Com o código abaixo definimos que o padrão de autenticação será o Nuget package JWTBEARER e como a API irá validar o token:

builder.Services.AddAuthentication(config =>
{
	config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(config =>
{
	config.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = false,
		ValidateAudience = false,
		ClockSkew = new TimeSpan(0),
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Settings:Jwt:SigningKey")!))

	};
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();


// app.UseAuthentication(); Precisamos dizer a API que precisamos fazer autenticações e definir como serão essas atenticações.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


if (builder.Configuration.IsTestEnvironment() == false)
{
	//Aqui é onde o método é chamando.
	//Ele fará a verificação se há uma migração, caso não, ele fará uma nova e logo após a applicação é executada
	await MigrateDatabase();
}


app.Run();

//Método criado para fazer a migration automaticamente:
//Ver Classe auxiliar na pasta Migrations em Infrastructure:
async Task MigrateDatabase()
{
	await using var scope = app.Services.CreateAsyncScope();

	await DataBaseMigration.MigrateDatabase(scope.ServiceProvider);
}


public partial class Program
{

}