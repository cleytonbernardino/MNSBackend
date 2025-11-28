using Microsoft.EntityFrameworkCore;
using MMS.Domain.Entities;
using MMS.Domain.Repositories.ServiceDefinition;

namespace MMS.Infrastructure.DataAccess.Repositories;

public class ServiceRepository(
    MmsDbContext dbContext
    ) : IServiceDefinitionWriteOnlyRepository
{
    private readonly MmsDbContext _dbContext = dbContext;

    #region WRITE ONLY

    public async Task Register(ServiceDefinition serviceDefinition)
    {
        await _dbContext.ServiceDefinitions.AddAsync(serviceDefinition);
    }

    #endregion
}
