using FluentMigrator;
using MMS.Domain.Enums;
using MMS.Domain.Security.Cryptography;

namespace MMS.Infrastructure.Migrations.Persistence;

[Migration(DatabaseVersions.REGISTER_INITIAL_USER, "Creating initial user")]
public class InitialUser(
    IPasswordEncrypter encrypter
    ) : ForwardOnlyMigration
{
    private readonly IPasswordEncrypter _encrypter = encrypter;

    public override void Up()
    {
        string hashedPassword = _encrypter.Encrypt("123456789");

        Insert.IntoTable(TableNames.TABLE_USER)
            .Row(new
            {
                IsAdmin = true,
                UpdatedOn = DateTime.UtcNow,
                LastLogin = DateTime.UtcNow,
                UserIdentifier = Guid.NewGuid(),
                Email = "admin@admin.com",
                Phone = "00000000000",
                FirstName = "delete-me",
                Password = hashedPassword,
                Role = (short)UserRolesEnum.ADMIN,
                CompanyId = 1
            });
    }
}
