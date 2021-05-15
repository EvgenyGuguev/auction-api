using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UseCases.Shared;

namespace UseCases.Account
{
    public class Register
    {
        public class Command : IRequest<Result<Unit>>
        {
            public RegisterDto User { get; set; }
        }
        
        public class CommandValidator : AbstractValidator<Command> 
        {
            public CommandValidator()
            {
                RuleFor(x => x.User).SetValidator(new UserRegisterValidator());
            }
        }
        
        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly UserManager<User> _userManager;

            public Handler(UserManager<User> userManager)
            {
                _userManager = userManager;
            }
            
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = new User
                {
                    FirstName = request.User.FirstName,
                    LastName = request.User.LastName,
                    Email = request.User.Email,
                    UserName = request.User.Username
                };

                var result = await _userManager.CreateAsync(user, request.User.Password);

                if (result.Succeeded) return Result<Unit>.Success(Unit.Value);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(error => error.Description).ToList();
                    // return Result<Unit>.Failure(JsonSerializer.Serialize(errors));
                    return Result<Unit>.Failure(errors);
                }

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}