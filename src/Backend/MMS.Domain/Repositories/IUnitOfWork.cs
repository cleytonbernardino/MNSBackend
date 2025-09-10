namespace MMS.Domain.Repositories;

public interface IUnitOfWork
{
    Task Commit();
}
