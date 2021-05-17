using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UseCases.Shared;

namespace UseCases.Account
{
    public class Register
    {
        public class Command : IRequest<Result<UserDto>>
        {
            public RegisterDto Dto { get; set; }
        }
        
        public class CommandValidator : AbstractValidator<Command> 
        {
            public CommandValidator(UserManager<User> userManager)
            {
                RuleFor(x => x.Dto).SetValidator(new UserRegisterValidator(userManager));
            }
        }
        
        public class Handler : IRequestHandler<Command, Result<UserDto>>
        {
            private readonly UserManager<User> _userManager;
            private readonly IMapper _mapper;

            public Handler(UserManager<User> userManager, IMapper mapper)
            {
                _userManager = userManager;
                _mapper = mapper;
            }
            
            public async Task<Result<UserDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = new User
                {
                    FirstName = request.Dto.FirstName,
                    LastName = request.Dto.LastName,
                    Email = request.Dto.Email,
                    UserName = request.Dto.Username
                };

                var result = await _userManager.CreateAsync(user, request.Dto.Password);

                var userToReturn = _mapper.Map<UserDto>(user);
                
                if (!result.Succeeded) return Result<UserDto>.Failure("Some problems with registration...");

                return Result<UserDto>.Success(userToReturn);
            }
        }
    }
}