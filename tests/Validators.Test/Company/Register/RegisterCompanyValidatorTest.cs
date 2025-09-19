using MMS.Application.UseCases.Company.Register;
using CommonTestUtilities.Requests;
using MMS.Exceptions;
using Shouldly;

namespace Validators.Test.Company.Register;

public class RegisterCompanyValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = RequestRegisterCompanyBuilder.Build();

        RegisterCompanyValidator validator = new();
        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Success_Without_DBA()
    {
        var request = RequestRegisterCompanyBuilder.Build();
        request.DoingBusinessAs = string.Empty;

        RegisterCompanyValidator validator = new();
        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Success_Without_Business_Email()
    {
        var request = RequestRegisterCompanyBuilder.Build();
        request.BusinessEmail = string.Empty;

        RegisterCompanyValidator validator = new();
        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Success_Without_WebSite()
    {
        var request = RequestRegisterCompanyBuilder.Build();
        request.WebSite = string.Empty;

        RegisterCompanyValidator validator = new();
        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_CNPJ_Not_Valid()
    {
        var request = RequestRegisterCompanyBuilder.Build();
        request.CNPJ = "750068710001";

        RegisterCompanyValidator validator = new();
        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors
            .Single()
            .ToString()
            .ShouldBe(ResourceMessagesException.CNPJ_INVALID);
    }

    [Fact]
    public void Error_CNPJ_Empty()
    {
        var request = RequestRegisterCompanyBuilder.Build();
        request.CNPJ = string.Empty;

        RegisterCompanyValidator validator = new();
        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors
            .Single()
            .ToString()
            .ShouldBe(ResourceMessagesException.CNPJ_EMPTY);
    }

    [Fact]
    public void Error_Legal_Name_Empty()
    {
        var request = RequestRegisterCompanyBuilder.Build();
        request.LegalName = string.Empty;

        RegisterCompanyValidator validator = new();
        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors
            .Single()
            .ToString()
            .ShouldBe(ResourceMessagesException.LEGAL_NAME_EMPTY);
    }

    [Fact]
    public void Error_CEP_Not_Valid()
    {
        var request = RequestRegisterCompanyBuilder.Build();
        request.CEP = "0814173";

        RegisterCompanyValidator validator = new();
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
        var request = RequestRegisterCompanyBuilder.Build();
        request.CEP = string.Empty;

        RegisterCompanyValidator validator = new();
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
        var request = RequestRegisterCompanyBuilder.Build();
        request.AddressNumber = string.Empty;

        RegisterCompanyValidator validator = new();
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
        var request = RequestRegisterCompanyBuilder.Build();
        request.PhoneNumber = string.Empty;

        RegisterCompanyValidator validator = new();
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
        var request = RequestRegisterCompanyBuilder.Build();
        request.PhoneNumber = "11 9873-1345";

        RegisterCompanyValidator validator = new();
        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors
            .Single()
            .ToString()
            .ShouldBe(ResourceMessagesException.PHONE_NOT_VALID);
    }
}
