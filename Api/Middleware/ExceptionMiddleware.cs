using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Serilog;
using UseCases.Shared;

namespace Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.Error(e, "{Message}", e.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                await HandlingException(e, context);
            }
        }

        private async Task HandlingException(Exception exception, HttpContext context)
        {
            var response = _env.IsDevelopment()
                ? new AppException(context.Response.StatusCode, exception.Message, exception.StackTrace)
                : new AppException(context.Response.StatusCode, exception.Message, "Server Error");

            var option = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var jsonResponse = JsonSerializer.Serialize(response, option);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}