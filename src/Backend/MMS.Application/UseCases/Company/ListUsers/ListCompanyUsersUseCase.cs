using MMS.Application.Extensions;
using MMS.Application.Services.Encoders;
using MMS.Communication;
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

    public async Task<ResponseListCompanyUser> Execute()
    {
        var loggedUser = await _loggedUser.User();
        CanGetUsers(loggedUser);

        var users = _repository.ListUsers(loggedUser.CompanyId);
        var response = new ResponseListCompanyUser();
        foreach(var user in users)
        {
            var shortUser = user.ToShortResponse();
            shortUser.Id = _idEncoder.Encode(user.Id);
            response.Users.Add(shortUser);
        };

        return response;
    }

    private static void CanGetUsers(Entity.User loggedUser)
    {
        if (loggedUser.IsAdmin)
            return;

        List<UserRolesEnum> validRoles = [
            UserRolesEnum.MANAGER, UserRolesEnum.SUB_MANAGER, UserRolesEnum.RH
            ];

        bool roleIsValid = validRoles.Any(role => loggedUser.Role == role);
        if (!roleIsValid)
            throw new NoPermissionException();
    }
}
