using Entities.Models;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Repository.Extensions
{
    public class OrderByQueryBuilder<T> where T : IEntity
    {
        public IQueryable<T> Source { get; set; }
        public string OrderByStrFromRequest { get; set; }

        public OrderByQueryBuilder(IQueryable<T> source, string orderByStrFromRequest)
        {
            this.Source = source;
            this.OrderByStrFromRequest = orderByStrFromRequest;
        }

        public string BuildOrderByQuery()
        {
            if (string.IsNullOrWhiteSpace(OrderByStrFromRequest))
                return null;

            StringBuilder orderByBuilder = BuildQueryWithAllParams();

            var orderQuery = orderByBuilder.ToString().TrimEnd(',', ' ');

            return orderQuery;
        }

        private StringBuilder BuildQueryWithAllParams()
        {
            var propertiInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderParams = OrderByStrFromRequest.Trim().Split(',');
            var orderByBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                PropertyInfo objectProperty = GetObjectProperty(propertiInfos, param);

                if (IsPropertyExist(objectProperty))
                    continue;

                string direction = SetupOrderByDirection(param);

                orderByBuilder.Append($"{objectProperty.Name} {direction},");
            }

            return orderByBuilder;
        }

        private static bool IsPropertyExist(PropertyInfo objectProperty)
            => objectProperty == null;

        private static PropertyInfo GetObjectProperty(PropertyInfo[] propertiInfos, string param)
        {
            var propertyFromQueryName = param.Split(" ")[0];
            var objectProperty = propertiInfos.FirstOrDefault(pi =>
                pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
            return objectProperty;
        }

        private static string SetupOrderByDirection(string param)
            => param.EndsWith(" desc") ?
                "descending" :
                "ascending";
    }
}
