using FluentValidation;
using MMS.Communication;
using MMS.Exceptions;

namespace MMS.Application.UseCases.Auth.RefreshToken;

public class RefreshTokenValidator : AbstractValidator<RequestRefreshAccessToken>
{
    public RefreshTokenValidator()
    {
        RuleFor(req => req.AccessToken).NotEmpty().WithMessage(ResourceMessagesException.INVALID_ACCESS_TOKEN);
    }
}
