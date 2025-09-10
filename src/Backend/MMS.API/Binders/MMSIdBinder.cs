using Microsoft.AspNetCore.Mvc.ModelBinding;
using MMS.Application.Services.Encoders;

namespace MMS.API.Binders;

public class MmsIdBinder(
    IIdEncoder idEncoder
    ) : IModelBinder
{
    private readonly IIdEncoder _idEncoder = idEncoder;

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var modelName = bindingContext.ModelName;

        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
            return Task.CompletedTask;

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        var value = valueProviderResult.FirstValue;

        if (string.IsNullOrWhiteSpace(value))
            return Task.CompletedTask;

        var id = _idEncoder.Decode(value);
        bindingContext.Result = ModelBindingResult.Success(id);

        return Task.CompletedTask;
    }
}
