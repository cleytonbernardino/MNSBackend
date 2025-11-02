using MMS.Application.Extensions;
using MMS.Application.Services.Encoders;
using MMS.Communication.Responses.User;
using MMS.Domain.Enums;
using MMS.Domain.Repositories.Company;
using MMS.Domain.Services.LoggedUser;
using MMS.Exceptions.ExceptionsBase;
using Entity = MMS.Domain.Entities;

namespace MMS.Application.UseCases.Company.ListUsers;

public class ListCompanyUsersUseCase(
    IIdEncoder idEncoder,
    ILoggedUser loggedUser,
    ICompanyReadOnlyRepository repository
) : IListCompanyUsersUseCase
{
    private readonly IIdEncoder _idEncoder = idEncoder;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly ICompanyReadOnlyRepository _repository = repository;
    
    public async Task<ResponseListShortUsers> Execute(long companyId)
    {
        var loggedUser = await _loggedUser.User();
        CanGetUsers(loggedUser, companyId);

        var users = _repository.ListUsers(companyId);
        var response = new ResponseListShortUsers();
        foreach(var user in users)
        {
            var shortUser = user.ToShortResponse();
            shortUser.Id = _idEncoder.Encode(user.Id);
            response.Users.Add(shortUser);
        };
        
        return response;
    }

    private static void CanGetUsers(Entity.User loggedUser, long companyId)
    {
        if (loggedUser.IsAdmin)
            return;

        List<UserRolesEnum> validRoles =
        [
            UserRolesEnum.MANAGER, UserRolesEnum.SUB_MANAGER, UserRolesEnum.RH
        ];

        bool roleIsValid = validRoles.Any(role => loggedUser.Role == role);
        if (!roleIsValid || loggedUser.CompanyId != companyId)
            throw new NoPermissionException();
    }
}
