using Contracts;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace CargoTransportationAPI.ActionFilters
{
    public class ValidateOrderExistsAttribute : ValidateExistsAttributeBase
    {
        public ValidateOrderExistsAttribute(ILoggerManager logger, IRepositoryManager repository)
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
            var id = (int)context.ActionArguments["orderId"];
            var order = await repository.Orders.GetOrderByIdAsync(id, trackChanges);

            FilterAttribute attribute = new FilterAttribute
            {
                Entity = order,
                EntityId = id,
                EntityName = nameof(order)
            };
            return attribute;
        }
    }
}
