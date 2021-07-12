using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UseCases.Shared;

namespace UseCases.Account
{
    public class RefreshToken
    {
        public class Query : IRequest<Result<User>>
        {
            public RefreshTokenDto Dto { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<User>>
        {
            private readonly IHttpContextAccessor _contextAccessor;
            private readonly UserManager<User> _userManager;
            private readonly IMapper _mapper;

            public Handler(IHttpContextAccessor contextAccessor, UserManager<User> userManager, IMapper mapper)
            {
                _contextAccessor = contextAccessor;
                _userManager = userManager;
                _mapper = mapper;
            }

            public async Task<Result<User>> Handle(Query request, CancellationToken cancellationToken)
            {
                var refreshToken = _contextAccessor.HttpContext?.Request.Cookies["refreshToken"];
                if (string.IsNullOrEmpty(refreshToken)) refreshToken = request.Dto.RefreshToken;

                var httpContextUser = _contextAccessor.HttpContext?.User;
                
                var user = await _userManager.Users
                    .Include(r => r.RefreshTokens)
                    .FirstOrDefaultAsync(x => x.UserName == httpContextUser.FindFirstValue(ClaimTypes.Name));

                if (user == null) return Result<User>.Failure("User not found");

                var oldToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);
                if (oldToken == null || !oldToken.IsActive) return Result<User>.Failure("Refresh Token not valid");

                return Result<User>.Success(user);
            }
        }
    }
}