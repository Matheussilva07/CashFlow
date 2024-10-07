using CashFlow.Application.useCases.Expenses.Reports.PDF.Colors;
using CashFlow.Application.useCases.Expenses.Reports.PDF.Fonts;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Reflection;
using CashFlow.Domain.Extensions;

namespace CashFlow.Application.useCases.Expenses.Reports.PDF;
public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
{
    private const string CURRENCY_SYMBOL = "R$";
    private const int HEIGHT_ROW_EXPENSE_TABLE = 25;
    
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

        CreateHeaderWithProfilePhotoAndName(page);

		var totalExpenses = expenses.Sum(expenses => expenses.Amount);

		CreateTotalSpentSection(page,totalExpenses, month);

        foreach (var expense in expenses)
        {
            var table = CreateExpensesTable(page);
            

            //Primeira linha da tabela:

            var row = table.AddRow();
            row.Height = HEIGHT_ROW_EXPENSE_TABLE;

			//Com a propriedade Cells nós acessamos as colunas de uma linha:		

            AddExpenseTitle(row.Cells[0], expense.Title);

            AddHeaderForAmount(row.Cells[3]);

			row = table.AddRow();
			row.Height = HEIGHT_ROW_EXPENSE_TABLE;

			row.Cells[0].AddParagraph(expense.Date.ToString("D"));

            SetStyleBaseForExpenseInformation(row.Cells[0]);
            row.Cells[0].Format.LeftIndent= 20;

			row.Cells[1].AddParagraph(expense.Date.ToString("t"));
            SetStyleBaseForExpenseInformation(row.Cells[1]);

			row.Cells[2].AddParagraph(expense.PaymentType.PaymentTypeToString());
            SetStyleBaseForExpenseInformation(row.Cells[2]);

            AddAmountForExpense(row.Cells[3], expense.Amount);

            if (string.IsNullOrWhiteSpace(expense.Description) == false)
            {
				var descriptionRow = table.AddRow();
				descriptionRow.Height = HEIGHT_ROW_EXPENSE_TABLE;

				descriptionRow.Cells[0].AddParagraph(expense.Description);
				descriptionRow.Cells[0].Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
				descriptionRow.Cells[0].Shading.Color = ColorsHelper.GREEN_LIGHT;
				descriptionRow.Cells[0].VerticalAlignment = VerticalAlignment.Center; 
				descriptionRow.Cells[0].MergeRight = 2;  //Esta propriedade recebe o número de colunas que serão mescladas/unidas da direita;
				descriptionRow.Cells[0].Format.LeftIndent = 20; //LeftIndent serve para dar um padding na esquerda;

                row.Cells[3].MergeDown = 1; //Esta propriedadade faz a msclagem das celulas abaixo;
			}

            AddWhiteEspace(table);
		}

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
    private void CreateHeaderWithProfilePhotoAndName(Section page)
    {
        //Código que cria uma tabela, assim como duas colunas e uma linha.
        //Por meio de seus objetos podemos acessar algumas propriedades e assim fazer outras configurações:

		var table = page.AddTable();
		table.AddColumn();
		table.AddColumn(300);

		var row = table.AddRow();

        //Código que pega o caminho da imagem:

        var assembly = Assembly.GetExecutingAssembly();
        var directoryName = Path.GetDirectoryName(assembly.Location);
        var pathFile = Path.Combine(directoryName!, "useCases\\Expenses\\Reports\\PDF\\Logo", "meuavatar.png");

        //Código que cria e define o tamanho da imagem:

        var imagem = row.Cells[0].AddImage(pathFile);
		imagem.Width = 64;
		imagem.Height = 64;

        //Código que cria um paragráfo e define uma fonte e alinhamento para ele:

		row.Cells[1].AddParagraph("Hey, Matheus Santana");
		row.Cells[1].Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 16 };
		row.Cells[1].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
	}

    //Código que cria uma seção com o total gasto no mês:
    private void CreateTotalSpentSection(Section page,decimal totalExpenses, DateOnly month)
    {

		var paragraph = page.AddParagraph();
		paragraph.Format.SpaceBefore = 40;
		paragraph.Format.SpaceAfter = 40;

		var title = string.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, month.ToString("Y"));

		paragraph.AddFormattedText(title, new Font { Name = FontHelper.RALEWAY_REGULAR, Size = 15 });

		paragraph.AddLineBreak();

		paragraph.AddFormattedText($"{totalExpenses} {CURRENCY_SYMBOL}", new Font { Name = FontHelper.WORKSANS_BLACK, Size = 50 });

	}

    //Código que cria a tabela e adiciona algumas colunas, estas devem, sempre, vir antes das linhas:
    private Table CreateExpensesTable(Section page)
    {
        var table = page.AddTable();

        table.AddColumn(195).Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn(80).Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn(120).Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn(120).Format.Alignment = ParagraphAlignment.Right;


        return table;
	}
	private void AddExpenseTitle(Cell cell, string expenseTitle)
	{
		cell.AddParagraph(expenseTitle);
		cell.Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = ColorsHelper.BLACK };
		cell.Shading.Color = ColorsHelper.RED_LIGHT;
		cell.VerticalAlignment = VerticalAlignment.Center;  // Tem tambem o Format.Alignment;
		cell.MergeRight = 2; //Esta propriedade recebe o número de colunas que serão mescladas/unidas da direita;
		cell.Format.LeftIndent = 20; //Este propriedade dá um padding à esquerda;
	}
    private void AddHeaderForAmount(Cell cell)
	{
		cell.AddParagraph(ResourceReportGenerationMessages.AMOUNT);
		cell.Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = ColorsHelper.WHITE };
		cell.Shading.Color = ColorsHelper.RED_DARK;
		cell.VerticalAlignment = VerticalAlignment.Center;
	}
	private void AddAmountForExpense(Cell cell, decimal amount)
    {
		cell.AddParagraph($"-{amount} {CURRENCY_SYMBOL}");
		cell.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 14, Color = ColorsHelper.BLACK };
		cell.Shading.Color = ColorsHelper.WHITE;
		cell.VerticalAlignment = VerticalAlignment.Center;
	}
    private void SetStyleBaseForExpenseInformation(Cell cell)
    {
		cell.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 12, Color = ColorsHelper.BLACK };
		cell.Shading.Color = ColorsHelper.GREEN_DARK;
		cell.VerticalAlignment = VerticalAlignment.Center;
	}
	private void AddWhiteEspace(Table table)
	{
		var row = table.AddRow(); //Esta linha foi criada para dar um espaço entre as tabelas;
		row.Height = 30;
		row.Borders.Visible = false;
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
