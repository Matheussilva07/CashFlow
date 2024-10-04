namespace CashFlow.Application.useCases.Expenses.Reports.PDF;
public interface IGenerateExpensesReportPdfUseCase
{
	Task<byte[]> Execute(DateOnly month);
}
