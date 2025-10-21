using MMS.Application.UseCases.SubscriptionPlan.Register;
using CommonTestUtilities.Requests;
using Shouldly;

namespace Validators.Test.SubscriptionPlan.Register;

public class RegisterSubscriptionPlanValidatorTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterSubscriptionPlanBuilder.Build();   
        RegisterSubscriptionPlanValidator validator = new();

        var result = await validator.ValidateAsync(request);
        result.IsValid.ShouldBeTrue();
    }
    
    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterSubscriptionPlanBuilder.Build();
        request.Name = string.Empty;
        
        RegisterSubscriptionPlanValidator validator = new();

        var result = await validator.ValidateAsync(request);
        result.IsValid.ShouldBeFalse();
    }
    
    [Fact]
    public async Task Error_Description_Empty()
    {
        var request = RequestRegisterSubscriptionPlanBuilder.Build();
        request.Description = string.Empty;
        
        RegisterSubscriptionPlanValidator validator = new();

        var result = await validator.ValidateAsync(request);
        result.IsValid.ShouldBeFalse();
    }
}
