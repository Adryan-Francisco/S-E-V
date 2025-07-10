using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace SEV.Utils
{
    public class DecimalModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext context)
        {
            var value = context.ValueProvider.GetValue(context.ModelName).ToString();

            if (decimal.TryParse(value, NumberStyles.Any, new CultureInfo("pt-BR"), out decimal result))
            {
                context.Result = ModelBindingResult.Success(result);
            }
            else
            {
                context.ModelState.TryAddModelError(context.ModelName, "Formato inválido.");
            }

            return Task.CompletedTask;
        }
    }

    public class DecimalModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(decimal) || context.Metadata.ModelType == typeof(decimal?))
            {
                return new DecimalModelBinder();
            }

            return null;
        }
    }
}
