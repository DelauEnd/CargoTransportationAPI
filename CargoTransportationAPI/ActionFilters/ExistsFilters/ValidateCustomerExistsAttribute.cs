using Contracts;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargoTransportationAPI.ActionFilters
{
    public class ValidateCustomerExistsAttribute : ValidateExistsAttributeBase
    {
        public ValidateCustomerExistsAttribute(ILoggerManager logger, IRepositoryManager repository)
        {
            base.logger = logger;
            base.repository = repository;
        }

        protected override async Task<FilterAttribute> GetAttributeAsync(ActionExecutingContext context)
        {
            FilterAttribute attribute = await BuildAttribute(context);

            return attribute;
        }
        private async Task<FilterAttribute> BuildAttribute(ActionExecutingContext context)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT");
            var id = (int)context.ActionArguments["customerId"];
            var customer = await repository.Customers.GetCustomerByIdAsync(id, trackChanges);

            FilterAttribute attribute = new FilterAttribute
            {
                Entity = customer,
                EntityId = id,
                EntityName = nameof(customer)
            };
            return attribute;
        }
    }
}
