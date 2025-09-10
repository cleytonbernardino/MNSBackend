using MMS.Application.Extensions;
using MMS.Application.Services.Encoders;
using MMS.Communication;
using MMS.Domain.Repositories.Company;

namespace MMS.Application.UseCases.Company.List;

public class ListCompaniesUseCase(
    ICompanyReadOnlyRepository repository,
    IIdEncoder idEncoder
    ) : IListCompaniesUseCase
{
    private readonly ICompanyReadOnlyRepository _repository = repository;
    private readonly IIdEncoder _idEncoder = idEncoder;

    public ResponseShortCompanies Execute()
    {
        var companies = _repository.ListCompanies();

        var responseShortCompanies = new ResponseShortCompanies();

        foreach (var company in companies )
        {
            var shortCompany = company.ToResponse();
            shortCompany.Id = _idEncoder.Encode(company.Id);

            responseShortCompanies.Companies.Add(shortCompany);
        }

        return responseShortCompanies;
    }
}
