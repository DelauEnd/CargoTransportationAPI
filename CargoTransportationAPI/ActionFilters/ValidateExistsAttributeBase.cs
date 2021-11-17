using CargoTransportationAPI.ActionFilters;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CargoTransportationAPI.ActionFilters
{
    public abstract class ValidateExistsAttributeBase : IAsyncActionFilter
    {
        protected IRepositoryManager repository;
        protected ILoggerManager logger;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await IsEntityExists(context, next);
        }

        private async Task IsEntityExists(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            FilterAttribute attribute = await GetAttributeAsync(context);

            if (attribute.Entity == null)
                NotFoundResult(context, attribute);
            else
                await AddEntityToContext(context, next, attribute);
        }

        protected abstract Task<FilterAttribute> GetAttributeAsync(ActionExecutingContext context);

        private void NotFoundResult(ActionExecutingContext context, FilterAttribute attribute)
        {
            var message = $"The desired object({attribute.EntityName}) with id {attribute.EntityId} was not found";
            logger.LogInfo(message);
            context.Result = new NotFoundResult();
        }

        private static async Task AddEntityToContext(ActionExecutingContext context, ActionExecutionDelegate next, FilterAttribute attribute)
        {
            context.HttpContext.Items.Add(attribute.EntityName, attribute.Entity);
            await next();
        }
    }
}
