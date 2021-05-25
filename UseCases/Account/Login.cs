using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UseCases.Shared;

namespace UseCases.Account
{
    public class Login
    {
        public class Query : IRequest<Result<User>>
        {
            public LoginDto Dto { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Dto).SetValidator(new UserLoginValidator());
            }
        }

        public class Handler : IRequestHandler<Query, Result<User>>
        {
            private readonly UserManager<User> _userManager;
            private readonly SignInManager<User> _signInManager;

            public Handler(UserManager<User> userManager, SignInManager<User> signInManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
            }

            public async Task<Result<User>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.Users
                    .FirstOrDefaultAsync(x => x.Email == request.Dto.Email);

                if (user == null) return Result<User>.Failure("Unauthorized");

                var result = await _signInManager
                    .CheckPasswordSignInAsync(user, request.Dto.Password, false);

                return result.Succeeded
                    ? Result<User>.Success(user)
                    : Result<User>.Failure("Unauthorized");
            }
        }
    }
}