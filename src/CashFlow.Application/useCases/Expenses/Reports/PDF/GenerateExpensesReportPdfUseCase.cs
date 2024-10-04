using CashFlow.Application.useCases.Expenses.Reports.PDF.Fonts;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using DocumentFormat.OpenXml.Drawing.Charts;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;
using PdfSharp.Fonts;

namespace CashFlow.Application.useCases.Expenses.Reports.PDF;
public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
{
    private const string CURRENCY_SYMBOL = "R$";
    
    private readonly IExpensesReadOnlyRepository _repository;
    public GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository repository)
    {
        this._repository = repository;

        //O código abaixo é que fará o gerenciamento das fontes

        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }
    public async Task<byte[]> Execute(DateOnly month)
	{
		var expenses = await _repository.FilterByMonth(month);

        if (expenses.Count == 0)
        {
            return [];
        }

        var document = CreateDocument(month);
        var page = CreatePage(document);

        var paragraph = page.AddParagraph();
        var title = string.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, month.ToString("Y"));

        //var foto = paragraph.AddImage("C:\\Users\\theus\\OneDrive\\Imagens\\meuavatar-transformed.png");
        //foto.Width = 100;
        //foto.Height = 100;
        //paragraph.AddLineBreak();

        paragraph.AddFormattedText(title, new Font { Name = FontHelper.RALEWAY_REGULAR, Size = 15 });

        paragraph.AddLineBreak();

        var totalExpenses = expenses.Sum(expenses => expenses.Amount);

        paragraph.AddFormattedText($"{totalExpenses} {CURRENCY_SYMBOL}", new Font { Name = FontHelper.WORKSANS_BLACK, Size = 50});

        return RenderDocument(document);
	}
    private Document CreateDocument(DateOnly month)
    {
        //Aqui é onde criamos o documento e podemos passar várias informações, nome da empresa, assunto e etc.

        var document = new Document();
        document.Info.Title = $"{month}";
        document.Info.Subject = "Faturamento";
        document.Info.Author = "Matheus Santana";

        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.RALEWAY_REGULAR;
        //style.Font.Size = 14;


        return document;
    }
    private Section CreatePage(Document document)
    {
       //A função AddSection serve para criar uma página PDF e com as propriedades abaixo podemos configurá-la:
        
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();

        section.PageSetup.PageFormat = PageFormat.A4;

        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;

        return section;
    }

    //O método abaixo é responsável por renderizar o documento:
    private byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer
        {
            Document = document
        };

        renderer.RenderDocument();

        using var file = new MemoryStream();

        renderer.PdfDocument.Save(file);

        return file.ToArray();
    }

}
