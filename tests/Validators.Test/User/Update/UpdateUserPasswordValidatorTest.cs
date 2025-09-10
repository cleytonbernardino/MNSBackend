using Shouldly;
using CommonTestUtilities.Requests;
using MMS.Application.UseCases.User.Update.Password;
using MMS.Exceptions;

namespace Validators.Test.User.Update;

public class UpdateUserPasswordValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = RequestUpdateUserPasswordBuilder.Build();
        
        UpdateUserPasswordValidator validator = new();
        var result = validator.Validate(request);
        result.IsValid.ShouldBeTrue();
    } 
    
    [Fact]
    public void Error_Password_Too_Short()
    {
        var request = RequestUpdateUserPasswordBuilder.Build();
        request.NewPassword = "12";
        
        UpdateUserPasswordValidator validator = new();
        var result = validator.Validate(request);
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.PASSWORD_LENGTH_IS_INVALID);
    }
    
    [Fact]
    public void Error_Old_Password_Is_Empty()
    {
        var request = RequestUpdateUserPasswordBuilder.Build();
        request.OldPassword = string.Empty;
        
        UpdateUserPasswordValidator validator = new();
        var result = validator.Validate(request);
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.PASSWORD_EMPTY);
    }
    
    [Fact]
    public void Error_New_Password_Is_Empty()
    {
        var request = RequestUpdateUserPasswordBuilder.Build();
        request.NewPassword = string.Empty;
        
        UpdateUserPasswordValidator validator = new();
        var result = validator.Validate(request);
        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors
            .ShouldHaveSingleItem()
            .ToString()
            .ShouldBe(ResourceMessagesException.PASSWORD_EMPTY);
    }
}
