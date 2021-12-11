using Entities;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Collections.Generic;
using System.Text;

namespace CargoTransportationAPI.Formatters
{
    public class CsvOutputFormatter : OutputFormatterBase
    {
        protected override string MediaType { get; set; } = "text/csv";

        protected override void BuildResponseMessage(StringBuilder responseMessage, OutputFormatterWriteContext context)
        {
            if (context.Object is IModelFormatter model)
                Append(responseMessage, model);
            else if (context.Object is IEnumerable<IModelFormatter> models)
                Append(responseMessage, models);
        }

        private void Append(StringBuilder responseMessage, IModelFormatter model)
        {
            responseMessage.Append(model.FormatToCsv());
        }

        private void Append(StringBuilder responseMessage, IEnumerable<IModelFormatter> models)
        {
            foreach (var model in models)
                responseMessage.Append(model.FormatToCsv()).Append(",\"");
            responseMessage.Remove(responseMessage.Length - 2, 2);
        }
    }
}
