using Domain;
using FluentValidation;

namespace UseCases.Account
{
    public class UserRegisterValidator : AbstractValidator<RegisterDto>
    {
        public UserRegisterValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(10);
            RuleFor(x => x.Username).NotEmpty();
        }
    }
}