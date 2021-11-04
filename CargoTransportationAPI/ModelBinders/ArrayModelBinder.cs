using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CargoTransportationAPI.ModelBinders
{
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!IsEnumerable(bindingContext))
                return BindingResult(bindingContext, ModelBindingResult.Failed());

            if (ContextValueIsNullOrEmpty(bindingContext))
                return BindingResult(bindingContext, ModelBindingResult.Success(null));

            SetupBindingContextModel(bindingContext);

            return BindingResult(bindingContext, ModelBindingResult.Success(bindingContext.Model));
        }     

        private static bool IsEnumerable(ModelBindingContext bindingContext)
        {
            return bindingContext.ModelMetadata.IsEnumerableType;
        }

        private static Task BindingResult(ModelBindingContext bindingContext, ModelBindingResult result)
        {
            bindingContext.Result = result;
            return Task.CompletedTask;
        }

        private static bool ContextValueIsNullOrEmpty(ModelBindingContext bindingContext)
        {
            string providedValue = GetProvidedValue(bindingContext);
            return string.IsNullOrEmpty(providedValue);
        }

        private static string GetProvidedValue(ModelBindingContext bindingContext)
        {
            return bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName)
                .ToString();
        }       

        private static void SetupBindingContextModel(ModelBindingContext bindingContext)
        {
            Array array = CreateArray(bindingContext);
            bindingContext.Model = array;
        }

        private static Array CreateArray(ModelBindingContext bindingContext)
        {
            var genericType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
            var converter = TypeDescriptor.GetConverter(genericType);
            var objectArray = GetProvidedValue(bindingContext).Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => converter.ConvertFromString(x.Trim()))
                .ToArray();
            var array = Array.CreateInstance(genericType, objectArray.Length);
            objectArray.CopyTo(array, 0);
            return array;
        }
    }
}
