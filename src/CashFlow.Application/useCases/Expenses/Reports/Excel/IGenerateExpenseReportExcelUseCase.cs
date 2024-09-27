namespace CashFlow.Application.useCases.Expenses.Reports.Excel;
public interface IGenerateExpenseReportExcelUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
