﻿using CashFlow.Application.useCases.Expenses;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Expenses;
public class ExpenseValidatorTests
{
    //O método abaixo é um teste de unidade e para que seja definido assim é necessário usar o atributo Fact

    [Fact]
    public void Success()
    {
        //Arrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();

        //Act
        var result = validator.Validate(request);

        //Assert
        //A classe Assert serve para fazer a verificação dos testes de unidade e ela é própria do .NET.
        //Mas podemos usar outros pacotes do NuGet.

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Error_Title_Empty(string title)
    {
        //Arrrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.Title = title;


        //Act
        var result = validator.Validate(request);

        //Assert     

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.TITLE_RIQUIRED));
    }

    [Fact]
    public void Error_Date_Future()
    {
        //Arrrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.Date = DateTime.UtcNow.AddDays(15);


        //Act
        var result = validator.Validate(request);

        //Assert     

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EXPENSES_CANNOT_FOR_THE_FUTURE));
    }

    [Fact]
    public void Error_Payment_Invalid()
    {
        //Arrrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.PaymentType = (CashFlow.Communication.Enums.PaymentType)70;


        //Act
        var result = validator.Validate(request);

        //Assert     

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.PAYMENTE_TYPE_INVALID));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    [InlineData(-11)]
    public void Error_Amount_Invalid(decimal amount)
    {
        //Arrrange
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.Amount = amount;


        //Act
        var result = validator.Validate(request);

        //Assert     

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO));
    }


	[Fact]
	public void Error_Tag_Invalid()
	{
		//Arrrange
		var validator = new ExpenseValidator();
		var request = RequestExpenseJsonBuilder.Build();
        request.Tags.Add((Tag)1000);


		//Act
		var result = validator.Validate(request);

		//Assert     

		result.IsValid.Should().BeFalse();
		result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.TAG_TYPE_NOT_SUPPORTED));
	}
}
