using Microsoft.OpenApi.Models;
using MMS.API.Binders;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MMS.API.Filter;

public class IdsFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var encryptedIds = context
            .ApiDescription
            .ParameterDescriptions
            .Where((x => x.ModelMetadata.BinderType == typeof(MmsIdBinder)))
            .ToDictionary(d => d.Name, d => d);

        foreach (var parameter in operation.Parameters)
        {
            if (encryptedIds.TryGetValue(parameter.Name, out _))
            {
                parameter.Schema.Format = string.Empty;
                parameter.Schema.Type = "string";
                ;
            }   
        }

        foreach (var schema in context.SchemaRepository.Schemas.Values)
        {
            foreach (var property in schema.Properties)
            {
                if (!encryptedIds.TryGetValue(property.Key, out _)) continue;
                property.Value.Format = string.Empty;
                property.Value.Type = "string";
            }
        }

    }
}
