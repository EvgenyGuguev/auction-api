using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using UseCases.Shared;

namespace UseCases.Account
{
    public class CurrentUser
    {
        public class Query : IRequest<Result<User>>
        {
        }

        public class Handler : IRequestHandler<Query, Result<User>>
        {
            private readonly UserManager<User> _userManager;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
            {
                _userManager = userManager;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<Result<User>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);

                return user == null
                    ? Result<User>.Failure("User not found")
                    : Result<User>.Success(user);
            }
        }
    }
}