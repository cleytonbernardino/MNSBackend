using MMS.Communication;

namespace MMS.Application.UseCases.Company.ListUsers;

public interface IListCompanyUsersUseCase
{
    Task<ResponseListCompanyUser> Execute();
}
