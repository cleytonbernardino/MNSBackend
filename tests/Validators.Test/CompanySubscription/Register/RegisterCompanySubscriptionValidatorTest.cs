using CommonTestUtilities.Requests;
using MMS.Application.UseCases.CompanySubscription.RegisterAndUpdate;
using Shouldly;

namespace Validators.Test.CompanySubscription.Register;

public class RegisterCompanySubscriptionValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = RequestRegisterCompanySubscriptionBuilder.Build();
        
        RegisterCompanySubscriptionValidator validator = new();
        var result = validator.Validate(request);
        
        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_Company_Id_Empty()
    {
        var request = RequestRegisterCompanySubscriptionBuilder.Build();
        request.CompanyId = string.Empty;
        
        RegisterCompanySubscriptionValidator validator = new();
        var result = validator.Validate(request);
        
        result.IsValid.ShouldBeFalse();
    }

    [Fact]
    public void Error_Subscription_Id_Empty()
    {
        var request = RequestRegisterCompanySubscriptionBuilder.Build();
        request.SubscriptionId = string.Empty;
        
        RegisterCompanySubscriptionValidator validator = new();
        var result = validator.Validate(request);
        
        result.IsValid.ShouldBeFalse();
    }
}
