using FluentValidation;

namespace UseCases.Account
{
    public class UserLoginValidator : AbstractValidator<LoginDto>
    {
        public UserLoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}