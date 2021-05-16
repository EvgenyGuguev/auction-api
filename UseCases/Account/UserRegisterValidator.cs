using Domain;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace UseCases.Account
{
    public class UserRegisterValidator : AbstractValidator<RegisterDto>
    {
        private readonly UserManager<User> _userManager;

        public UserRegisterValidator(UserManager<User> userManager)
        {
            _userManager = userManager;
            
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty()
                .MustAsync(async (email, cancellation) =>
                {
                    var exists = await userManager.Users.AnyAsync(x => x.Email == email);
                    return !exists;
                }).WithMessage("Email is already taken.");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(10);
            RuleFor(x => x.Username).NotEmpty()
                .MustAsync(async (username, cancellation) =>
                {
                    var exists = await userManager.Users.AnyAsync(x => x.UserName == username);
                    return !exists;
                }).WithMessage("Username is already taken.");;
        }
    }
}