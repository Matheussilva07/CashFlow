using DocumentFormat.OpenXml.Bibliography;
using System.Collections;

namespace WebAPI.Tests.InlineData;

public class CultureInlineDataTest : IEnumerable<object[]>
{
	public IEnumerator<object[]> GetEnumerator()
	{
		yield return new object[] { "fr" };
		yield return new object[] { "en" };
		yield return new object[] { "pt-BR" };
		yield return new object[] { "pt-PT" };

		//Quando usamos o returno, todo o código abaixo dele não será mais executado. Quando utilizamos a palavra chase yield podificamos essa condição.
		//Com o yield antes do return estavamos indicando que tal método deverá retornar algo, mas deverá continuar executando o código abaixo.
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	
	//O yield em C# é uma palavra-chave usada para produzir sequências de valores em um método ou em um bloco de código. 
	//Ele permite que um método retorne uma coleção de itens de forma preguiçosa (lazy), ou seja, os valores são gerados e
	//retornados um por um, sob demanda, sem precisar de toda a coleção ser criada e armazenada na memória de uma vez.
	//Quando você utiliza yield return, o método retorna um valor de forma sequencial e mantém o estado entre as chamadas,
	//permitindo que a iteração sobre esses valores seja feita de forma eficiente.
	

}

