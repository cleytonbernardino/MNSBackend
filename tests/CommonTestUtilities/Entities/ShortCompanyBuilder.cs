using Bogus;
using MMS.Domain.Entities;

namespace CommonTestUtilities.Entities;

public static class ShortCompanyBuilder
{
    public static ShortCompany Build()
    {
        return new Faker<ShortCompany>()
            .RuleFor(company => company.Id, 1)
            .RuleFor(company => company.DoingBusinessAs, f => f.Company.CompanyName())
            .RuleFor(company => company.SubscriptionPlan, f => f.Commerce.Product())
            .RuleFor(company => company.ManagerName, f => f.Name.FirstName())
            .RuleFor(company => company.SubscriptionStatus, true);
    }

    public static IList<ShortCompany> BuildInBatch(uint count = 5)
    {
        List<ShortCompany> companies = [];
        for (int i = 0; i < count; i++)
        {
            ShortCompany company = Build();
            company.Id = i++;
            companies.Add(company);
        }

        return companies;
    }
}
