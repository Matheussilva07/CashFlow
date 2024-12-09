using CashFlow.Application.useCases.Expenses.Delete;
using CashFlow.Application.useCases.Expenses.GetAll;
using CashFlow.Application.useCases.Expenses.GetById;
using CashFlow.Application.useCases.Expenses.Register;
using CashFlow.Application.useCases.Expenses.Update;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ExpensesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredExpenseJson) , StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson) , StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromServices] IRegisterExpenseUseCase useCase , [FromBody] RequestExpenseJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseExpensesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllExpenses([FromServices]IGetAllExpensesUseCase useCase)
    {
        var response = await useCase.Execute();

        if (response.Expenses.Count != 0)       
            return Ok(response);
        
        return NoContent();
        
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseExpenseJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson),StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromServices]IGetExpenseByIdUseCase useCase, [FromRoute]int id)
    {
        var response = await useCase.Execute(id);

        return Ok(response);        
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromServices]IDeleteExpenseUseCase useCase, [FromRoute] int id)
    {
        await useCase.Execute(id);

        return NoContent();
    }

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromServices]IUpdateExpenseUseCase useCase,[FromRoute] int id,[FromBody] RequestExpenseJson request)
    {
        await useCase.Execute(id,request);
        
        return NoContent();
    }
}
