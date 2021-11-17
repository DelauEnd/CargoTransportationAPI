using Contracts;
using Entities;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoTransportationAPI.Formatters
{
    public class CsvOutputFormatter : OutputFormatterBase
    {
        protected override string MediaType { get; set; } = "text/csv";

        protected override void BuildResponseMessage(StringBuilder responseMessage, OutputFormatterWriteContext context)
        {
            if (context.Object is IModelFormatter)
                Append(responseMessage, (IModelFormatter)context.Object);
            else if (context.Object is IEnumerable<IModelFormatter>)
                Append(responseMessage, (IEnumerable<IModelFormatter>)context.Object);               
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
