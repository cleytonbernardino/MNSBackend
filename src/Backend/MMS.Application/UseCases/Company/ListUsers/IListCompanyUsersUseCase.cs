using MMS.Communication.Responses.User;

namespace MMS.Application.UseCases.Company.ListUsers;

public interface IListCompanyUsersUseCase
{
    Task<ResponseListShortUsers> Execute(long companyId);
}
