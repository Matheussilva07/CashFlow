using CashFlow.API.Filters;
using CashFlow.API.Middleware;
using CashFlow.Application;
using CashFlow.Application.AutoMapper;
using CashFlow.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//O código abaixo é para definir o filtro das exceções que fizemos:
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));


//O código abaixo serve para criar as injeções de dependências em uma classe propria por meio de metodos de extensão:
builder.Services.AddInfrastructure(builder.Configuration);
//O código abaixo serve para criar as injeções de dependências em uma classe propria por meio de metodos de extensão:
builder.Services.AddApplication();

builder.Services.AddRouting(option => option.LowercaseUrls = true);

//O código abaixo foi inserido para configurar o Swagger:

builder.Services.AddSwaggerGen(options => {
	options.MapType<DateOnly>(() => new OpenApiSchema
	{
		Type = "string",
		Format = "date"
	});
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

app.UseAuthorization();

app.MapControllers();

app.Run();
