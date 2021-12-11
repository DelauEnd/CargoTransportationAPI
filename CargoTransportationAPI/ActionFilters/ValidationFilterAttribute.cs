using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace CargoTransportationAPI.ActionFilters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        private readonly ILoggerManager logger;

        public ValidationFilterAttribute(ILoggerManager logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            CheckContext(context);
        }

        private void CheckContext(ActionExecutingContext context)
        {
            var actionArgument = GetActionArgument(context);

            if (actionArgument == null)
                SendedIsNullResult(context);

            if (!context.ModelState.IsValid)
                InvalidModelStateResult(context);
        }

        private static object GetActionArgument(ActionExecutingContext context)
        {
            return context.ActionArguments
                            .SingleOrDefault(x => x.Value.ToString().Contains("Dto")).Value;
        }

        private void InvalidModelStateResult(ActionExecutingContext context)
        {
            var message = $"Object has incorrect state. Controller: {GetController(context)}, Action: {GetAction(context)}";
            logger.LogError(message);
            context.Result = new BadRequestObjectResult(message);
        }

        private void SendedIsNullResult(ActionExecutingContext context)
        {
            var message = $"Sended object is null. Controller: {GetController(context)}, Action: {GetAction(context)}";
            logger.LogError(message);
            context.Result = new BadRequestObjectResult(message);
        }

        private static string GetController(ActionExecutingContext context)
        {
            return (string)context.RouteData.Values["controller"];
        }

        private static string GetAction(ActionExecutingContext context)
        {
            return (string)context.RouteData.Values["action"];
        }
    }
}
