using MMS.Communication;
using MMS.Domain.Repositories.User;
using MMS.Domain.Security.Token;
using MMS.Exceptions.ExceptionsBase;

namespace MMS.Application.UseCases.Login.DoLogin;

public class DoLoginUseCase(
    IUserReadOnlyRepository repository,
    IRefreshTokenHandler refreshTokenHandler,
    IAccessTokenGenerator accessTokenGenerator
    ) : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _repository = repository;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;
    private readonly IRefreshTokenHandler _refreshTokenHandler = refreshTokenHandler;

    public async Task<ResponseRegisteredUser> Execute(RequestLogin request)
    {
        Validator(request);

        var user = await _repository.GetUserByEmailAndPassword(request.Email, request.Password)
            ?? throw new InvalidLoginException();

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

    private static void Validator(RequestLogin request)
    {
        DoLoginValidator validator = new();

        var result = validator.Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(
                result.Errors.Select(err => err.ErrorMessage).ToArray());
    }
}
