using Contracts;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace CargoTransportationAPI.ActionFilters
{
    public class ValidateCargoExistsAttribute : ValidateExistsAttributeBase
    {
        public ValidateCargoExistsAttribute(ILoggerManager logger, IRepositoryManager repository)
        {
            base.logger = logger;
            base.repository = repository;
        }

        protected override async Task<FilterAttribute> GetAttributeAsync(ActionExecutingContext context)
            => await BuildAttribute(context);

        private async Task<FilterAttribute> BuildAttribute(ActionExecutingContext context)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PATCH");
            var id = (int)context.ActionArguments["cargoId"];
            var cargo = await repository.Cargoes.GetCargoByIdAsync(id, trackChanges);

            FilterAttribute attribute = new FilterAttribute
            {
                Entity = cargo,
                EntityId = id,
                EntityName = nameof(cargo)
            };
            return attribute;
        }
    }
}
