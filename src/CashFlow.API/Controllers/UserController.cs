﻿using CashFlow.Application.useCases.Users.Profile;
using CashFlow.Application.useCases.Users.Register;
using CashFlow.Application.useCases.Users.Update;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
	[HttpPost]
	[ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Register([FromServices] IRegisterUserUseCase useCase,[FromBody]RequestRegisterUserJson request)
	{
		var response =await useCase.Execute(request);

		return Created(string.Empty,response);
	}

	[HttpGet]
	[Authorize]
	[ProducesResponseType(typeof(ResponseUserProfileJson),StatusCodes.Status200OK)]
	public async Task<IActionResult> GetProfile([FromServices] IGetUserProfileUseCase useCase)
	{
		var response = await useCase.Execute();

		return Ok(response);
	}

	[HttpPut]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> UpdateProfile([FromServices] IUpdateUserUseCase useCase , [FromBody] RequestUpdateUserJson request)
	{
		await useCase.Execute(request);
		
		return NoContent();
	}
	[HttpPut]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> ChangePassword([FromServices] IUpdateUserUseCase useCase, [FromBody] RequestUpdateUserJson request)
	{
		await useCase.Execute(request);

		return NoContent();
	}
}
