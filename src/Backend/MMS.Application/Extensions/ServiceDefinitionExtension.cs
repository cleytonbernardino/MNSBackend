using MMS.Communication.Requests.ServiceDefinition;
using MMS.Communication.Responses.ServiceDefinition;
using MMS.Domain.Enums;
using Entity = MMS.Domain.Entities;

namespace MMS.Application.Extensions;

public static class UserServicesExtension
{
    public static Entity.ServiceDefinition ToEntity(this RequestRegisterServices request)
    {
        return new Entity.ServiceDefinition
        {
            Id = 0,
            Active = true,
            Title = request.Title,
            Description = request.Description,
            ServiceType = request.ServiceType
        };
    }

    public static ResponseGetServiceDefinition ToResponse(this Entity.ServiceDefinition entity)
    {
        return new ResponseGetServiceDefinition
        {
            Title = entity.Title,
            Description = entity.Description ?? string.Empty,
            Status = (short)entity.Status,
            ServiceType = entity.ServiceType
        };
    }

    public static ResponseShortServiceDefinition[] ToResponse(this Entity.ShortServiceDefinition[] entities)
    {
        ResponseShortServiceDefinition[] response = new ResponseShortServiceDefinition[entities.Length];
        for (int i = 0; i < entities.Length; i++)
        {
            var shortServiceDefinition = new ResponseShortServiceDefinition
            {
                Title = entities[i].Title, Description = entities[i].Description!
            };
            response[i] = shortServiceDefinition;
        }

        return response;
    }

    public static Entity.ServiceDefinition Join(this Entity.ServiceDefinition entity,
        RequestUpdateServiceDefinition request)
    {
        entity.Title = request.Title ?? entity.Title;
        entity.Description = request.Description ?? entity.Description;
        entity.ServiceType = request.ServiceType ?? entity.ServiceType;

        if (request.Status is not null)
        {
            var result = (ServicesStatusEnum)request.Status;
            entity.Status = result;
        }

        return entity;
    }
}
