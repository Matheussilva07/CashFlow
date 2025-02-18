namespace CashFlow.Domain.Entities;
public class Tag
{
	public long Id { get; set; }
	public Enums.Tag Value { get; set; } //Foi colocado o diretório anterior 'Enums.' para não confundir com o nome da Classe Tag e dar conflito.
	public long ExpenseId { get; set; } 
	public Expense Expense { get; set; } = default!;
}
