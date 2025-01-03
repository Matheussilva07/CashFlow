﻿using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.useCases.Expenses.Register;
public interface IRegisterExpenseUseCase
{
   Task<ResponseRegisteredExpenseJson> Execute(RequestExpenseJson request);
}
