using Bogus;
using MMS.Communication.Requests.Company;

namespace CommonTestUtilities.Requests;

public static class RequestUpdateCompanyBuilder
{
    public static RequestUpdateCompany Build()
    {
        return new Faker<RequestUpdateCompany>()
            .RuleFor(req => req.DoingBusinessAs, f => f.Company.CompanySuffix())
            .RuleFor(req => req.CEP, f => f.Address.ZipCode("#####-###"))
            .RuleFor(req => req.AddressNumber, f => f.Address.BuildingNumber())
            .RuleFor(req => req.BusinessEmail, f => f.Internet.Email())
            .RuleFor(req => req.PhoneNumber, f => f.Phone.PhoneNumber("(##) 9####-####"))
            .RuleFor(req => req.WebSite, f => f.Internet.Url());
    }
}
