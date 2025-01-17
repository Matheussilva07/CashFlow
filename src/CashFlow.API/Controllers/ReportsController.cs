using CashFlow.Application.useCases.Expenses.Reports.Excel;
using CashFlow.Application.useCases.Expenses.Reports.PDF;
using CashFlow.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CashFlow.API.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.ADMIN)] //Este abributo define que para acessar esses endpoints é necessário ter o token e também ser admin.
public class ReportsController : ControllerBase
{
	[HttpGet("excel")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> GetExcel([FromServices] IGenerateExpenseReportExcelUseCase useCase, [FromHeader] DateOnly month)
	{
		byte[] file = await useCase.Execute(month);

		if (file.Length > 0)
			return File(file, MediaTypeNames.Application.Octet, "report.xlsx");

		return NoContent();
	}

	[HttpGet("pdf")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> GetPDF([FromServices] IGenerateExpensesReportPdfUseCase useCase, [FromQuery] DateOnly month)
	{
		byte[] file = await useCase.Execute(month);

		if (file.Length > 0)
		{
			return File(file, MediaTypeNames.Application.Pdf, "report.pdf");
		}

		return NoContent();
	}
}
