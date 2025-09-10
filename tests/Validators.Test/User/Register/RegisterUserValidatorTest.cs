using CommonTestUtilities.Requests;
using Shouldly;
using MMS.Application.UseCases.User.Register;
using MMS.Communication;
using MMS.Exceptions;

namespace Validators.Test.User.Register;

public class RegisterUserValidatorTest
{
    [Fact]
    public void Success()
    {
        RegisterUserValidator validator = new();

        RequestRegisterUser request = RequestRegisterUseBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Success_Without_Name()
    {
        RegisterUserValidator validator = new();

        RequestRegisterUser request = RequestRegisterUseBuilder.Build();
        request.LastName = "";

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_Email_Empty()
    {
        RegisterUserValidator validator = new();

        RequestRegisterUser request = RequestRegisterUseBuilder.Build();
        request.Email = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single()
            .ToString()
            .ShouldBe(ResourceMessagesException.EMAIL_EMPTY);
    }

    [Fact]
    public void Error_Email_Incorrect()
    {
        RegisterUserValidator validator = new();

        RequestRegisterUser request = RequestRegisterUseBuilder.Build();
        request.Email = "testing";

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single()
            .ToString()
            .ShouldBe(ResourceMessagesException.INVALID_EMAIL);
    }

    [Fact]
    public void Error_Phone_Incorrect()
    {
        RegisterUserValidator validator = new();

        RequestRegisterUser request = RequestRegisterUseBuilder.Build();
        request.Phone = "928194jh142";

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single()
            .ToString()
            .ShouldBe(ResourceMessagesException.PHONE_NOT_VALID);
    }

    [Fact]
    public void Error_Phone_Empty()
    {
        RegisterUserValidator validator = new();

        RequestRegisterUser request = RequestRegisterUseBuilder.Build();
        request.Phone = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single()
            .ToString()
            .ShouldBe(ResourceMessagesException.PHONE_EMPTY);
    }

    [Fact]
    public void Error_First_Name_Empty()
    {
        RegisterUserValidator validator = new();

        RequestRegisterUser request = RequestRegisterUseBuilder.Build();
        request.FirstName = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single()
            .ToString()
            .ShouldBe(ResourceMessagesException.FIRST_NAME_EMPTY);
    }

    [Fact]
    public void Error_Password_Empty()
    {
        RegisterUserValidator validator = new();

        RequestRegisterUser request = RequestRegisterUseBuilder.Build();
        request.Password = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single()
            .ToString()
            .ShouldBe(ResourceMessagesException.PASSWORD_EMPTY);
    }

    [Fact]
    public void Error_Role_Does_Not_Exist()
    {
        var request = RequestRegisterUseBuilder.Build();
        request.Role = 10000;

        RegisterUserValidator validator = new();
        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single()
            .ToString()
            .ShouldBe(ResourceMessagesException.ROLE_INVALID);
    }
}
