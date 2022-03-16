using Entities.ErrorModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace Logistics.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError => appError.Run
            (
                async context => await context.ContextConfigureAsync()
            ));
        }

        public static async Task ContextConfigureAsync(this HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            await context.HandleException();
        }

        public static async Task HandleException(this HttpContext context)
        {
            var contextFeauture = context.Features.Get<IExceptionHandlerFeature>();
            if (contextFeauture != null)
            {
                await context.Response.WriteAsync(new ErrorDetails()
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "Internal server error"
                }.ToString());
            }
        }
    }
}
