using CashFlow.Domain.Enums;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using ClosedXML.Excel;

namespace CashFlow.Application.useCases.Expenses.Reports.Excel;
public class GenerateExpenseReportExcelUseCase : IGenerateExpenseReportExcelUseCase
{
    private const string CURRENCY_SYMBOL = "R$";

    private readonly IExpensesReadOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
	public GenerateExpenseReportExcelUseCase(IExpensesReadOnlyRepository repository, ILoggedUser loggedUser)
	{
		_repository = repository;
		_loggedUser = loggedUser;
	}
	public async Task<byte[]> Execute(DateOnly month)
    {
        var loggedUser = await _loggedUser.GetUser();
        
        var expenses = await _repository.FilterByMonth(loggedUser, month);

        if (expenses.Count == 0)
        {
            return [];
        }

        using var workBook = new XLWorkbook();

        workBook.Author = loggedUser.Name;
        workBook.Style.Font.FontSize = 12;
        workBook.Style.Font.FontColor = XLColor.WarmBlack;
        workBook.Style.Font.FontName = "Arial";

        //O código abaixo é usado para criar uma folha excel e leva como parametro o nome dela;

       var workSheet = workBook.Worksheets.Add(month.ToString("Y"));

       //Código para criar o cabeçalho da planilha

        InsertHeader(workSheet);        

        var row = 2;

        foreach (var expense in expenses)
        {
            workSheet.Cell($"A{row}").Value = expense.Title;
            workSheet.Cell($"B{row}").Value = expense.Date;
            workSheet.Cell($"C{row}").Value = expense.PaymentType.PaymentTypeToString();
            workSheet.Cell($"D{row}").Value = expense.Amount;

            workSheet.Cell($"D{row}").Style.NumberFormat.Format = $"-{CURRENCY_SYMBOL} #,##0.00";
            
            workSheet.Cell($"E{row}").Value = expense.Description;

            row++;
        }

        //workSheet.Columns().AdjustToContents(); //No excel online não funciona

        var file = new MemoryStream();
        workBook.SaveAs(file);

        return file.ToArray();
    }

    private string ConvertPaymentType(PaymentType payment)
    {
        return payment switch
        {
            PaymentType.Cash => "DInheiro",
            PaymentType.CreditCard => "Cartão de crédito",
            PaymentType.DebitCard => "Cartão de débito",
            PaymentType.EletronicTransfer => "Transerência eletrónica",

            _=> string.Empty
        };

        //Essa conversão poderia ser feita de várias maneiras, por meio do ToString(), ou por um método de extensão
    }
    private void InsertHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = "Titulo";
        worksheet.Cell("B1").Value = "Data";
        worksheet.Cell("C1").Value = "Tipo de Pagamento";
        worksheet.Cell("D1").Value = "Quantidade";
        worksheet.Cell("E1").Value = "Descrição";

        worksheet.Cells("A1:E1").Style.Font.Bold = true;

        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#00ff00");

        worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    }
}
