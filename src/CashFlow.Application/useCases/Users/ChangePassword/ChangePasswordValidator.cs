using CashFlow.Application.useCases.Users;
using CashFlow.Communication.Requests;
using FluentValidation;

namespace CashFlow.Application.useCases.Users.ChangePassword;
public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangePasswordValidator()
    {
        RuleFor(user => user.NewPassword).SetValidator(new PasswordValidator<RequestChangePasswordJson>());
    }
}
