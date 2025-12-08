using Microsoft.EntityFrameworkCore;
using MMS.Domain.Entities;
using MMS.Domain.Repositories.ServiceDefinition;

namespace MMS.Infrastructure.DataAccess.Repositories;

public class ServiceRepository(
    MmsDbContext dbContext
    ) : IServiceDefinitionWriteOnlyRepository, IServiceDefinitionReadOnlyRepository, IServiceDefinitionUpdateOnlyRepository
{
    private readonly MmsDbContext _dbContext = dbContext;

    #region WRITE ONLY

    public async Task Register(ServiceDefinition serviceDefinition)
    {
        await _dbContext.ServiceDefinitions.AddAsync(serviceDefinition);
    }

    #endregion

    #region READ ONLY

    public async Task<ServiceDefinition?> GetById(User user, long id)
    {
        return await _dbContext.ServiceDefinitions
            .AsNoTracking()
            .FirstOrDefaultAsync(service =>
            service.Id == id && service.CompanyId == user.CompanyId);
    }

    public async Task<ShortServiceDefinition[]> List(User user)
    {
        return await _dbContext.ServiceDefinitions
            .AsNoTracking()
            .Where(service => service.CompanyId == user.CompanyId)
            .Select(service => new ShortServiceDefinition
            {
                Title = service.Title,
                Description = service.Description
            }).ToArrayAsync();
    }

    #endregion

    #region UPDATE ONLY

    async Task<ServiceDefinition?> IServiceDefinitionUpdateOnlyRepository.GetById(User user, long id)
    {
        return await _dbContext.ServiceDefinitions.FirstOrDefaultAsync(service =>
            service.Id == id && service.CompanyId == user.CompanyId);
    }
    
    public void Update(ServiceDefinition service)
    {
        _dbContext.ServiceDefinitions.Update(service);
    }
    
    public void Delete(ServiceDefinition serviceDefinition)
    {
        _dbContext.ServiceDefinitions.Remove(serviceDefinition);
    }

    #endregion
}
