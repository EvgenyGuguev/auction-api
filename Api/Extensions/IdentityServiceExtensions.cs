using DataAccess;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityCore<User>(opt =>
                {
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequiredLength = 10;
                    opt.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<DataContext>();

            return services;
        }
    }
}