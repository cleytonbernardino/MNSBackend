using CommonTestUtilities.Requests;
using Shouldly;
using MMS.Application.UseCases.User.Update;
using MMS.Exceptions;

namespace Validators.Test.User.Update;

public class UpdateUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = RequestUpdateUserBuilder.Build();

        UpdateUserValidator validator = new();
        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_Role_Invalid()
    {
        var request = RequestUpdateUserBuilder.Build();
        request.Role = 1000;

        UpdateUserValidator validator = new();
        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.ROLE_INVALID);
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var request = RequestUpdateUserBuilder.Build();
        request.Email = "Invalid";

        UpdateUserValidator validator = new();
        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.INVALID_EMAIL);
    }

    [Fact]
    public void Error_Phone_Invalid()
    {
        var request = RequestUpdateUserBuilder.Build();
        request.Phone = "119742ll89";

        UpdateUserValidator validator = new();
        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.PHONE_NOT_VALID);
    }
}
