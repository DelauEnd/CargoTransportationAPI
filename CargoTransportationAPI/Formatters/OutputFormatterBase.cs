using Entities;
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
    public abstract class OutputFormatterBase : TextOutputFormatter
    {
        protected List<Type> SupportedTypes { get; set; }
        protected abstract string MediaType { get; set; }

        public OutputFormatterBase()
        {
            ConfigureFormatter();
        }

        protected virtual void ConfigureFormatter()
        {
            SupportedMediaTypes.Add(MediaType);
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
            InitSupportedTypes();
        }

        private void InitSupportedTypes()
        {
            SupportedTypes = FormatterSupportedTypes.SupportedTypes;
        }

        protected override bool CanWriteType(Type type)
        {
            if (SupportedTypes.Any(supportedType => supportedType.IsAssignableFrom(type)))
            {
                return base.CanWriteType(type);
            }
            return false;
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var responseMessage = new StringBuilder();
            BuildResponseMessage(responseMessage, context);

            await response.WriteAsync(responseMessage.ToString());
        }

        protected abstract void BuildResponseMessage(StringBuilder responseMessage, OutputFormatterWriteContext context);
    }
}
