using CommonTestUtilities.Requests;
using CommonTestUtilities.Requests.ServicesDefinition;
using MMS.Application.UseCases.ServiceDefinition.Register;
using Shouldly;

namespace Validators.Test.ServiceDefinition.Register;

public class RegisterUserServicesValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = RequestRegisterServiceDefinitionBuilder.Build();
        
        RegisterServicesValidator validator = new();
        var result = validator.Validate(request);
        
        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_Title_Empty()
    {
        var request = RequestRegisterServiceDefinitionBuilder.Build();
        request.Title = string.Empty;

        RegisterServicesValidator validator = new();
        var result = validator.Validate(request);
        
        result.IsValid.ShouldBeFalse();
    }

    [Fact]
    public void Error_Service_Type_Empty()
    {
        var request = RequestRegisterServiceDefinitionBuilder.Build();
        request.ServiceType = string.Empty;

        RegisterServicesValidator validator = new();
        var result = validator.Validate(request);
        
        result.IsValid.ShouldBeFalse();
    }
}
