namespace MMS.Infrastructure.Migrations;

internal abstract class DatabaseVersions
{
    public const int TABLE_SUBSCRIPTIONS = 1;
    public const int TABLE_COMPANY = 2;
    public const int TABLE_USER = 3;
    public const int Add_Foreign_Keys = 4;
    public const int TABLE_REFRESH_TOKENS = 5;
    public const int CREATE_INITIAL_USER = 6; // Persistence
}
