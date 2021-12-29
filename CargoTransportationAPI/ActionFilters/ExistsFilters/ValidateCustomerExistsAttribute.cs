using Contracts;
using Microsoft.AspNetCore.Mvc.Filters;
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
            => await BuildAttribute(context);

        private async Task<FilterAttribute> BuildAttribute(ActionExecutingContext context)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PATCH");
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
