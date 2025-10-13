using MMS.Application.Extensions;
using MMS.Application.Services.Encoders;
using MMS.Communication.Responses.Company;
using MMS.Domain.Repositories.Company;
using MMS.Domain.Services.Cache;
using MMS.Domain.Services.LoggedUser;
using MMS.Exceptions.ExceptionsBase;

namespace MMS.Application.UseCases.Company.List;

public class ListCompaniesUseCase(
    ILoggedUser loggedUser,
    ICompanyReadOnlyRepository repository,
    ICacheService cache,
    IIdEncoder idEncoder
    ) : IListCompaniesUseCase
{
    private readonly IIdEncoder _idEncoder = idEncoder;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly ICompanyReadOnlyRepository _repository = repository;
    private readonly ICacheService _cache = cache;

    private const string CACHE_KEY = "Companies";
    
    public async Task<ResponseShortCompanies> Execute()
    {
        var cache  = await _cache.GetCache<ResponseShortCompanies>(CACHE_KEY);
        if (cache is not null)
            return cache;
        
        var loggedUser = await _loggedUser.User();

        if (loggedUser.IsAdmin == false)
            throw new NoPermissionException();

        var companies = _repository.ListShortCompanies();

        var responseShortCompanies = new ResponseShortCompanies();

        foreach (var company in companies )
        {
            var shortCompany = company.ToResponse();
            shortCompany.Id = _idEncoder.Encode(company.Id);

            responseShortCompanies.Companies.Add(shortCompany);
        }

        await _cache.SaveCache(CACHE_KEY, responseShortCompanies, expirationTime: 0);
        return responseShortCompanies;
    }
}
