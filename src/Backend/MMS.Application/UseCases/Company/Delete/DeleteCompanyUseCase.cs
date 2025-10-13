using Microsoft.Extensions.Logging;
using MMS.Domain.Repositories;
using MMS.Domain.Repositories.Company;
using MMS.Domain.Services.Cache;
using MMS.Domain.Services.LoggedUser;
using MMS.Exceptions.ExceptionsBase;

namespace MMS.Application.UseCases.Company.Delete;

public class DeleteCompanyUseCase(
    ILoggedUser loggedUser,
    ICompanyWriteOnlyRepository repository,
    ICacheService cache,
    IUnitOfWork unitOfWork,
    ILogger<DeleteCompanyUseCase> logger
    ) : IDeleteCompanyUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly ICompanyWriteOnlyRepository _repository = repository;
    private readonly ICacheService _cache = cache;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<DeleteCompanyUseCase> _logger = logger;
    
    private const string CACHE_KEY = "Companies";
        
    public async Task Execute(long id)
    {
        var loggedUser = await _loggedUser.User();

        _logger.LogInformation(
            $"Foi Requisitado que a empresa de id: {id}, fosse apagada.\nPelo Admin:{loggedUser.FirstName}"
            );
        if (loggedUser.IsAdmin == false)
        {
            _logger.LogCritical("Acesso não permitido ao deletar uma empresa.\n--- Token Jwt Comprometido --");
            throw new NoPermissionException();
        }

        await _repository.Delete(id);
        await _unitOfWork.Commit();
        await _cache.DeleteCache(CACHE_KEY);
    }
}
