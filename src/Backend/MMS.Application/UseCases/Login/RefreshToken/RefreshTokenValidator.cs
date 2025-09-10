using FluentValidation;
using MMS.Communication;
using MMS.Exceptions;

namespace MMS.Application.UseCases.Login.RefreshToken;

public class RefreshTokenValidator : AbstractValidator<RequestRefreshToken>
{
    public RefreshTokenValidator()
    {
        RuleFor(req => req.AccessToken).NotEmpty().WithMessage(ResourceMessagesException.INVALID_ACCESS_TOKEN);
        RuleFor(req => req.RefreshToken).NotEmpty().WithMessage(ResourceMessagesException.INVALID_REFRESH_TOKEN);
    }
}
