using Bogus;
using MMS.Domain.Dtos;

namespace CommonTestUtilities.Dto;
public static class WhatsAppMessageDtoBuilder
{
    public static WhatsAppMessageDto Build()
    {
        return new Faker<WhatsAppMessageDto>()
            .RuleFor(dto => dto.Messages, f => f.Lorem.Paragraph())
            .RuleFor(dto => dto.UserIdentifier, f => f.Phone.PhoneNumber("###########"));
    }
}
