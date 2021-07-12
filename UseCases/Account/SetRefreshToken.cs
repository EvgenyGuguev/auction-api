using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using UseCases.Shared;


namespace UseCases.Account
{
    public class SetRefreshToken
    {
        public class Command : IRequest<Result<User>>
        {
            public User User { get; set; }
        }
        
        // public class Handler : IRequestHandler<Command, Result<User>>
        // {
        //     public async Task<Result<User>> Handle(Command request, CancellationToken cancellationToken)
        //     {
        //         var refreshToken = CreateRefreshToken();
        //     
        //         request.RefreshTokens.Add(refreshToken);
        //     }
        // }
    }
}