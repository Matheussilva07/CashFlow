using CashFlow.Domain.Enums;

namespace CashFlow.Domain.Extensions;
public static class PaymentTypeExtensions
{
	public static string PaymentTypeToString(this PaymentType paymentType)
	{
		return paymentType switch
		{
			PaymentType.Cash => "DInheiro",
			PaymentType.CreditCard => "Cartão de crédito",
			PaymentType.DebitCard => "Cartão de débito",
			PaymentType.EletronicTransfer => "Transerência eletrónica",
			_ => string.Empty
		};

	}
}
