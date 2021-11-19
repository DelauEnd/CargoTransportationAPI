using Entities.Models;
using Entities.RequestFeautures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Repository.Extensions
{
    public class OrderByQueryBuilder<T> where T : IEntity
    {
        public IQueryable<T> source { get; set; }
        public string orderByStrFromRequest { get; set; }

        public OrderByQueryBuilder(IQueryable<T> source, string orderByStrFromRequest)
        {
            this.source = source;
            this.orderByStrFromRequest = orderByStrFromRequest;
        }

        public string BuildOrderByQuery()
        {
            if (string.IsNullOrWhiteSpace(orderByStrFromRequest))
                return null;

            StringBuilder orderByBuilder = BuildQueryWithAllParams();

            var orderQuery = orderByBuilder.ToString().TrimEnd(',', ' ');

            return orderQuery;
        }

        private StringBuilder BuildQueryWithAllParams()
        {
            var propertiInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderParams = orderByStrFromRequest.Trim().Split(',');
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
        {
            return objectProperty == null;
        }

        private static PropertyInfo GetObjectProperty(PropertyInfo[] propertiInfos, string param)
        {
            var propertyFromQueryName = param.Split(" ")[0];
            var objectProperty = propertiInfos.FirstOrDefault(pi =>
                pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));
            return objectProperty;
        }

        private static string SetupOrderByDirection(string param)
        {
            return param.EndsWith(" desc") ?
                "descending" :
                "ascending";
        }
    }
}
