using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GreekTheater.API.Helpers.CustomModelBinders
{
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            var valuesAsString = bindingContext.ValueProvider
                            .GetValue(bindingContext.ModelName)
                            .ToString();

            if (string.IsNullOrWhiteSpace(valuesAsString))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            SetModelToValuesArray(bindingContext, valuesAsString);

            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);

            return Task.CompletedTask;
        }

        private void SetModelToValuesArray(ModelBindingContext bindingContext, string valuesAsString)
        {
            var elementType = GetTypeOfValues(bindingContext);
            var values = ConvertValuesToTheirType(elementType, valuesAsString);

            bindingContext.Model = GetArrayOfValues(elementType, values);
        }

        private Type GetTypeOfValues(ModelBindingContext bindingContext)
        {
            return bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
        }

        private object[] ConvertValuesToTheirType(Type elementType, string value)
        {
            var converter = TypeDescriptor.GetConverter(elementType);

            return value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => converter.ConvertFromString(x.Trim()))
                .ToArray();
        }

        private Array GetArrayOfValues(Type elementType, object[] values)
        {
            var typedValues = Array.CreateInstance(elementType, values.Length);
            values.CopyTo(typedValues, 0);

            return typedValues;
        }
    }
}
