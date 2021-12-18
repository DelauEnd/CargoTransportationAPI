using Contracts;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace CargoTransportationAPI.ActionFilters
{
    public class ValidateTransportExistsAttribute : ValidateExistsAttributeBase
    {
        public ValidateTransportExistsAttribute(ILoggerManager logger, IRepositoryManager repository)
        {
            base.logger = logger;
            base.repository = repository;
        }

        protected override async Task<FilterAttribute> GetAttributeAsync(ActionExecutingContext context)
            => await BuildAttribute(context);

        private async Task<FilterAttribute> BuildAttribute(ActionExecutingContext context)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT");
            var id = (int)context.ActionArguments["transportId"];
            var transport = await repository.Transport.GetTransportByIdAsync(id, trackChanges);

            FilterAttribute attribute = new FilterAttribute
            {
                Entity = transport,
                EntityId = id,
                EntityName = nameof(transport)
            };
            return attribute;
        }
    }
}
