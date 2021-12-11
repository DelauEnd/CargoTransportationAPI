using Entities.Models;
using Entities.RequestFeautures;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Repository.Extensions
{
    public static class CargoRepositoryExtensions
    {
        public static IQueryable<Cargo> ApplyFilters(this IQueryable<Cargo> cargoes, CargoParameters parameters)
            => cargoes.Where(cargo => cargo.DepartureDate >= parameters.ArrivalDateFrom && 
            cargo.DepartureDate <= parameters.ArrivalDateTo &&
            cargo.DepartureDate >= parameters.DepartureDateFrom &&
            cargo.DepartureDate <= parameters.DepartureDateTo);

        public static IQueryable<Cargo> Search(this IQueryable<Cargo> cargoes, string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return cargoes;

            var searchValues = search.Trim().ToLower();

            return cargoes.Where(cargoes =>
                cargoes.Title.Contains(searchValues) ||
                cargoes.Category.Title.Contains(searchValues));
        }

        public static IQueryable<Cargo> Sort(this IQueryable<Cargo> cargoes, CargoParameters parameters)
        {
            OrderByQueryBuilder<Cargo> builder = new OrderByQueryBuilder<Cargo>(cargoes, parameters.OrderBy);

            var orderQuery = builder.BuildOrderByQuery();

            if (string.IsNullOrWhiteSpace(orderQuery))
                return cargoes;

            return cargoes.OrderBy(orderQuery);
        }
    }
}
