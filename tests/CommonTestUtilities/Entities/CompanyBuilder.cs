using Bogus;
using MMS.Domain.Entities;

namespace CommonTestUtilities.Entities;

public static class CompanyBuilder
{
    public static Company Build(User? manager = null)
    {
        Faker<Company> company = new Faker<Company>()
            .RuleFor(company => company.Id, 0)
            .RuleFor(company => company.UpdatedOn, () => DateTime.UtcNow)
            .RuleFor(company => company.CNPJ, f => f.Random.Replace("##.###.###/####-##"))
            .RuleFor(company => company.LegalName, f => f.Company.CompanyName())
            .RuleFor(company => company.DoingBusinessAs, f => f.Company.CompanySuffix())
            .RuleFor(company => company.CEP, f => f.Address.ZipCode("#####-###"))
            .RuleFor(company => company.AddressNumber, f => f.Address.BuildingNumber())
            .RuleFor(company => company.BusinessEmail, f => f.Internet.Email())
            .RuleFor(company => company.PhoneNumber, f => f.Phone.PhoneNumber("(##) #####-####"))
            .RuleFor(company => company.CompanySubscription, () => null)
            .RuleFor(company => company.SubscriptionStatus, true);

        if (manager is not null)
        {
           // company.RuleFor(company => company.Manager, manager);
            company.RuleFor(company => company.ManagerId, manager.Id);
        }
            
        
        return company.Generate();
    }

    public static Company[] BuildInBatch(User? manager = null, uint count = 5, bool withId = false)
    {
        List<Company> companies = [];
        for (int i = 1; i <= count; i++)
        {
            Company company = Build(manager);
            if (withId)
                company.Id = i;
            companies.Add(company);
        }
        
        return companies.ToArray();
    }
}
