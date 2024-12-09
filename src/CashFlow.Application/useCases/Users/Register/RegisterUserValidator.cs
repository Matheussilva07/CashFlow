using CashFlow.Communication.Requests;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.useCases.Users.Register;
public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress()
            .When(user => string.IsNullOrWhiteSpace(user.Email) == false, ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourceErrorMessages.EMAIL_INVALID);

        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
	}
}

//RuleFor(user => user.Email)
//.NotEmpty().WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
//.EmailAddress()
//.When(user => string.IsNullOrWhiteSpace(user.Email) == false, ApplyConditionTo.CurrentValidator)
//.WithMessage(ResourceErrorMessages.EMAIL_INVALID);

//Quando usamos o When após um validator estamos definindo uma condição para que ele seja verificado, neste caso, ele só será verificado caso
//o email não seja vazio, no entanto, é necessário definir para quem essa condição será aplicada, por essa razão devemos colocar um segundo paramentro,
//que é o  ApplyConditionTo.CurrentValidator, com ele estamos definindo que essa condição será aplicada tão somente ao validator atual.