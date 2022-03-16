using Interfaces;
using Entities.ErrorModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace CargoTransportationAPI.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
        {
            app.UseExceptionHandler(appError => appError.Run
            (
                async context => await context.ContextConfigureAsync(logger)
            ));
        }

        public static async Task ContextConfigureAsync(this HttpContext context, ILoggerManager logger)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            await context.HandleException(logger);
        }

        public static async Task HandleException(this HttpContext context, ILoggerManager logger)
        {
            var contextFeauture = context.Features.Get<IExceptionHandlerFeature>();
            if (contextFeauture != null)
            {
                logger.LogError($"Something went wrong: {contextFeauture.Error}");

                await context.Response.WriteAsync(new ErrorDetails()
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Internal server error"
                }.ToString());
            }
        }
    }
}
