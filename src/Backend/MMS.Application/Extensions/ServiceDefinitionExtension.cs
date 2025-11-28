using MMS.Communication.Requests.ServiceDefinition;

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
}
