using Api.Services;
using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using UseCases.Account;
using UseCases.Shared;

namespace Api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo 
                {Title = "Api", Version = "v1"}); 
            });

            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("PostgreSqlConnection"));
            });

            services.AddMediatR(typeof(Register.Handler).Assembly);
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);

            services.AddScoped<AccountService>();

            return services;
        }
    }
}