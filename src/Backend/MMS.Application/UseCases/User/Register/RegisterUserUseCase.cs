using FluentValidation.Results;
using MMS.Application.Extensions;
using MMS.Communication;
using MMS.Domain.Enums;
using MMS.Domain.Repositories;
using MMS.Domain.Repositories.User;
using MMS.Domain.Security.Cryptography;
using MMS.Domain.Security.Token;
using MMS.Domain.Services.LoggedUser;
using MMS.Exceptions;
using MMS.Exceptions.ExceptionsBase;
using Entity = MMS.Domain.Entities;

namespace MMS.Application.UseCases.User.Register;

public class RegisterUserUseCase(
    ILoggedUser loggedUser,
    IUserReadOnlyRepository readOnlyRepository,
    IUserWriteOnlyRepository writeOnlyRepository,
    IPasswordEncrypter encrypter,
    IAccessTokenGenerator accessTokenGenerator,
    IRefreshTokenHandler refreshTokenHandler,
    IUnitOfWork unitOfWork
    ) : IRegisterUserUseCase
{

    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUserReadOnlyRepository _readOnlyRepository = readOnlyRepository;
    private readonly IRefreshTokenHandler _refreshTokenHandler = refreshTokenHandler;
    private readonly IUserWriteOnlyRepository _writeOnlyRepository = writeOnlyRepository;
    private readonly IPasswordEncrypter _encrypter = encrypter;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;
    private readonly IUnitOfWork _unityOfWork = unitOfWork;

    public async Task<ResponseRegisteredUser> Execute(RequestRegisterUser request)
    {
        var loggedUser = await _loggedUser.User();

        bool canCreate = CanCreateUser(loggedUser, (UserRolesEnum)request.Role);
        if (!canCreate)
            throw new NoPermissionException();

        await Validate(request);

        Entity.User user = request.ToUser();
        user.UpdatedOn = DateTime.UtcNow;
        user.LastLogin = DateTime.UtcNow;
        user.UserIdentifier = Guid.NewGuid();
        user.Password = _encrypter.Encrypt(request.Password);
        user.CompanyId = loggedUser.CompanyId;

        await _writeOnlyRepository.RegisterUser(user);
        await _unityOfWork.Commit();
        
        return new ResponseRegisteredUser
        {
            FirstName = user.FirstName,
            Tokens = new ResponseToken
            {
                AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier, user.Role),
                RefreshToken = await _refreshTokenHandler.GenerateTokenAndSave(user.UserIdentifier)
            }
        };
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

    private static bool CanCreateUser(Entity.User loggedUser, UserRolesEnum userTargetRole)
    {
        if (loggedUser.IsAdmin || loggedUser.Role == UserRolesEnum.MANAGER && userTargetRole != UserRolesEnum.ADMIN)
            return true;

        return loggedUser.Role > userTargetRole &&
             userTargetRole != UserRolesEnum.CUSTOMER &&
             userTargetRole != UserRolesEnum.EMPLOYEE;
    }
}
