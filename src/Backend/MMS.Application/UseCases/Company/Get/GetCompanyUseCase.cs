using MMS.Application.Extensions;
using MMS.Communication.Responses.Company;
using MMS.Domain.Enums;
using MMS.Domain.Repositories.Company;
using MMS.Domain.Services.LoggedUser;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
using Entity = MMS.Domain.Entities;

namespace MMS.Application.UseCases.Company.Get;

public class GetCompanyUseCase(
    ILoggedUser loggedUser,
    ICompanyReadOnlyRepository repository
    ) : IGetCompanyUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly ICompanyReadOnlyRepository _repository = repository;
    
    public async Task<ResponseCompany> Execute(long id)
    {
        var loggedUser = await _loggedUser.User();
        HasPermission(loggedUser);

        var company = await _repository.GetById(loggedUser, id);
        if (company is null)
            throw new NotFoundException(ResourceMessagesException.COMPANY_NOT_FOUND);

        return company.ToResponse();
    }

    private static void HasPermission(Entity.User user)
    {
        if (user.IsAdmin || user.Role == UserRolesEnum.MANAGER) return;

        throw new NoPermissionException();
    }
}
