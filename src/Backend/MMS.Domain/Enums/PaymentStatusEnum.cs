namespace MMS.Domain.Enums;

public enum PaymentStatusEnum : short
{
    PENDING = 0,
    PAID = 1,
    FAILED = 2,
    CANCELED = 3,
    REFUNDED = 4,
    EXPIRED = 5
}
