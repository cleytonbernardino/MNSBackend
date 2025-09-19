using FluentValidation;
using MMS.Communication.Requests.Auth;
using MMS.Exceptions;

namespace MMS.Application.UseCases.Auth.RefreshToken;

public class RefreshTokenValidator : AbstractValidator<RequestRefreshToken>
{
    public RefreshTokenValidator()
    {
        RuleFor(req => req.AccessToken).NotEmpty().WithMessage(ResourceMessagesException.INVALID_ACCESS_TOKEN);
    }
}
