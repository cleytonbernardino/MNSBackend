using MMS.Communication.Requests.Auth;
using MMS.Communication.Responses.Auth;
using MMS.Domain.Repositories.User;
using MMS.Domain.Security.Token;
using MMS.Exceptions.ExceptionsBase;

namespace MMS.Application.UseCases.Auth.DoLogin;

public class DoLoginUseCase(
    IUserReadOnlyRepository repository,
    IRefreshTokenHandler refreshTokenHandler,
    IAccessTokenGenerator accessTokenGenerator
    ) : IDoLoginUseCase
{
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;
    private readonly IRefreshTokenHandler _refreshTokenHandler = refreshTokenHandler;
    private readonly IUserReadOnlyRepository _repository = repository;

    private string? _refreshToken;

    public async Task<ResponseDoLogin> Execute(RequestDoLogin request)
    {
        Validator(request);

        var user = await _repository.GetUserByEmailAndPassword(request.Email, request.Password)
            ?? throw new InvalidLoginException();

        _refreshToken = await _refreshTokenHandler.GenerateTokenAndSave(user.UserIdentifier);
        return new ResponseDoLogin
        {
            FirstName = user.FirstName, AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier, user.Role)
        };
    }

    public string? GetRefreshToken() => _refreshToken;

    private static void Validator(RequestDoLogin request)
    {
        DoLoginValidator validator = new();

        var result = validator.Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(
                result.Errors.Select(err => err.ErrorMessage).ToArray());
    }
}
