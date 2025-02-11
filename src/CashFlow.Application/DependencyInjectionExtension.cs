using CashFlow.Application.AutoMapper;
using CashFlow.Application.useCases.Expenses.Delete;
using CashFlow.Application.useCases.Expenses.GetAll;
using CashFlow.Application.useCases.Expenses.GetById;
using CashFlow.Application.useCases.Expenses.Register;
using CashFlow.Application.useCases.Expenses.Reports.Excel;
using CashFlow.Application.useCases.Expenses.Reports.PDF;
using CashFlow.Application.useCases.Expenses.Update;
using CashFlow.Application.useCases.Login;
using CashFlow.Application.useCases.Users.Profile;
using CashFlow.Application.useCases.Users.Register;
using CashFlow.Application.useCases.Users.Update;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Application;
public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddAutoMapper(services);
        AddUseCases(services);
    }
       
    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapping));
    }
    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterExpenseUseCase, RegisterExpenseUseCase>();
        services.AddScoped<IGetAllExpensesUseCase, GetAllExpensesUseCase>();
        services.AddScoped<IGetExpenseByIdUseCase, GetExpenseByIdUseCase>();
        services.AddScoped<IDeleteExpenseUseCase, DeleteExpenseUseCase>();
        services.AddScoped<IUpdateExpenseUseCase, UpdateExpenseUseCase>();
        services.AddScoped<IGenerateExpenseReportExcelUseCase, GenerateExpenseReportExcelUseCase>();
        services.AddScoped<IGenerateExpensesReportPdfUseCase, GenerateExpensesReportPdfUseCase>();
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();

        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();

        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
         
    }
}
