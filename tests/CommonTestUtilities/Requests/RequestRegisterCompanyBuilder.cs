using Bogus;
using MMS.Communication;

namespace CommonTestUtilities.Requests;

public static class RequestRegisterCompanyBuilder
{
    public static RequestRegisterCompany Build()
    {
        return new Faker<RequestRegisterCompany>()
            .RuleFor(req => req.CNJP, () => "75006871000111")
            .RuleFor(req => req.LegalName, f => f.Company.CompanyName())
            .RuleFor(req => req.DoingBusinessAs, f => f.Name.LastName())
            .RuleFor(req => req.CEP, () => "08141730")
            .RuleFor(req => req.AddressNumber, () => "22")
            .RuleFor(req => req.BusinessEmail, f => f.Internet.Email())
            .RuleFor(req => req.PhoneNumber, () => "11987125344")
            //.RuleFor(req => req.WhatsappAPINumber, () => "11987125344")
            .RuleFor(req => req.SubscriptionStatus, () => true)
            .RuleFor(req => req.Website, () => @"https://google.com");
    }
}
