using MMS.Domain.Repositories;

namespace MMS.Infrastructure.DataAccess;

public class UnitOfWork(
    MmsDbContext dbContext
    ) : IUnitOfWork
{
    private readonly MmsDbContext _dbContext = dbContext;

    public async Task Commit() => await _dbContext.SaveChangesAsync();
}
