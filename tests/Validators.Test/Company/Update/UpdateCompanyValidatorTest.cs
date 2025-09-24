using CommonTestUtilities.Requests;
using MMS.Application.UseCases.Company.Update;
using MMS.Exceptions;
using Shouldly;

namespace Validators.Test.Company.Update;

public class UpdateCompanyValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = RequestUpdateCompanyBuilder.Build();

        UpdateCompanyValidator validator = new();
        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }
    
    [Fact]
    public void Error_DoingBusinessAs_Empty()
    {
        var request = RequestUpdateCompanyBuilder.Build();
        request.DoingBusinessAs = string.Empty;

        UpdateCompanyValidator validator = new();
        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors
            .Single()
            .ToString()
            .ShouldBe(ResourceMessagesException.DBA_EMPTY);
    }
    
    [Fact]
    public void Error_DoingBusinessAs_Max_Length()
    {
        var request = RequestUpdateCompanyBuilder.Build();
        request.DoingBusinessAs = new string('A', 200);
        
        UpdateCompanyValidator validator = new();
        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors
            .Single()
            .ToString()
            .ShouldBe(ResourceMessagesException.DBA_MAX_LENGTH);
    }
    
    [Fact]
    public void Error_CEP_Not_Valid()
    {
        var request = RequestUpdateCompanyBuilder.Build();
        request.CEP = "0814173";

        UpdateCompanyValidator validator = new();
        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors
            .Single()
            .ToString()
            .ShouldBe(ResourceMessagesException.CEP_INVALID);
    }

    [Fact]
    public void Error_CEP_Empty()
    {
        var request = RequestUpdateCompanyBuilder.Build();
        request.CEP = string.Empty;

        UpdateCompanyValidator validator = new();
        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors
            .Single()
            .ToString()
            .ShouldBe(ResourceMessagesException.CEP_EMPTY);
    }

    [Fact]
    public void Error_Address_Number_Empty()
    {
        var request = RequestUpdateCompanyBuilder.Build();
        request.AddressNumber = string.Empty;

        UpdateCompanyValidator validator = new();
        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors
            .Single()
            .ToString()
            .ShouldBe(ResourceMessagesException.ADDRESS_NUMBER_EMPTY);
    }

    [Fact]
    public void Error_Phone_Number_Empty()
    {
        var request = RequestUpdateCompanyBuilder.Build();
        request.PhoneNumber = string.Empty;

        UpdateCompanyValidator validator = new();
        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors
            .Single()
            .ToString()
            .ShouldBe(ResourceMessagesException.PHONE_EMPTY);
    }

    [Fact]
    public void Error_Phone_Number_Invalid()
    {
        var request = RequestUpdateCompanyBuilder.Build();
        request.PhoneNumber = "11 9873-1345";

        UpdateCompanyValidator validator = new();
        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors
            .Single()
            .ToString()
            .ShouldBe(ResourceMessagesException.PHONE_NOT_VALID);
    }
}
