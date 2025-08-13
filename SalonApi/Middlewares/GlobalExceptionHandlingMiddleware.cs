
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace VideoStore.Api.Middlewares
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private const string ERROR = @"A Internal Server Error has occurred: {0}";
        private readonly ILogger _logger;

        public GlobalExceptionHandlingMiddleware(ILogger logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(string.Format(ERROR, e.Message));
            }
        }
    }
}
