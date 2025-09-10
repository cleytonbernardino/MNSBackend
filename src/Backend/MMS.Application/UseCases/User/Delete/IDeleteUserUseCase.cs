namespace MMS.Application.UseCases.User.Delete;

public interface IDeleteUserUseCase
{
    Task Execute(long userToDeleteId);
}
