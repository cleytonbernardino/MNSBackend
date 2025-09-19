using CommonTestUtilities.Requests;
using MMS.Application.UseCases.Auth.DoLogin;
using Shouldly;

namespace Validators.Test.Auth;

public class DoLoginValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = RequestLoginBuilder.Build();

        DoLoginValidator validator = new();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_Email_Empty()
    {
        var request = RequestLoginBuilder.Build();
        request.Email = string.Empty;

        DoLoginValidator validator = new();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var request = RequestLoginBuilder.Build();
        request.Email = "email";

        DoLoginValidator validator = new();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
    }

    [Fact]
    public void Error_Password_Invalid()
    {
        var request = RequestLoginBuilder.Build();
        request.Password = string.Empty;

        DoLoginValidator validator = new();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
    }
}
