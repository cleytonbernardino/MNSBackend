using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using MMS.Application.Extensions;
using MMS.Communication.Requests.User;
using MMS.Domain.Enums;
using MMS.Domain.Repositories;
using MMS.Domain.Repositories.User;
using MMS.Domain.Security.Cryptography;
using MMS.Domain.Services.Cache;
using MMS.Domain.Services.LoggedUser;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
using Entity = MMS.Domain.Entities;

namespace MMS.Application.UseCases.User.Register;

public class RegisterUserUseCase(
    ILoggedUser loggedUser,
    IUserReadOnlyRepository readOnlyRepository,
    IUserWriteOnlyRepository writeOnlyRepository,
    ICacheService cache,
    IPasswordEncrypter encrypter,
    IUnitOfWork unitOfWork,
    ILogger<RegisterUserUseCase> logger
) : IRegisterUserUseCase
{
    private readonly IPasswordEncrypter _encrypter = encrypter;

    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly ILogger _logger = logger;
    private readonly IUserReadOnlyRepository _readOnlyRepository = readOnlyRepository;
    private readonly IUnitOfWork _unityOfWork = unitOfWork;
    private readonly IUserWriteOnlyRepository _writeOnlyRepository = writeOnlyRepository;
    private readonly ICacheService _cache = cache;
    
    public async Task Execute(RequestRegisterUser request)
    {
        var loggedUser = await _loggedUser.User();

        bool canCreate = CanCreateUser(loggedUser, (UserRolesEnum)request.Role);
        if (!canCreate)
            throw new NoPermissionException();

        await Validate(request);

        Entity.User user = request.ToUser();
        user.LastLogin = DateTime.UtcNow;
        user.UserIdentifier = Guid.NewGuid();
        user.Password = _encrypter.Encrypt(request.Password);
        user.CompanyId = loggedUser.CompanyId;

        if ((UserRolesEnum)request.Role == UserRolesEnum.ADMIN && loggedUser.IsAdmin)
            user.IsAdmin = true;

        await _writeOnlyRepository.RegisterUser(user);
        await _unityOfWork.Commit();
        await _cache.DeleteCache($"Companies/Users/id:{user.CompanyId}");
    }

    private async Task Validate(RequestRegisterUser request)
    {
        RegisterUserValidator validator = new();
        var result = await validator.ValidateAsync(request);

        bool emailExist = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);
        if (emailExist)
            result.Errors.Add(new ValidationFailure("", ResourceMessagesException.EMAIL_IN_USE));

        if ( !result.IsValid )
            throw new ErrorOnValidationException(
                result.Errors.Select(err => err.ErrorMessage).ToArray()
            );
    }

    private bool CanCreateUser(Entity.User loggedUser, UserRolesEnum userTargetRole)
    {
        if (loggedUser.IsAdmin)
        {
            _logger.LogWarning($"Admin: {loggedUser.FirstName}, Registrou um novo usuário.");
            return true;
        }

        if (loggedUser.Role == UserRolesEnum.MANAGER && userTargetRole != UserRolesEnum.ADMIN)
            return true;

        return loggedUser.Role > userTargetRole &&
             userTargetRole != UserRolesEnum.CUSTOMER &&
             userTargetRole != UserRolesEnum.EMPLOYEE;
    }
}
