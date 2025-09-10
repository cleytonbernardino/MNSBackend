using FluentMigrator;
using MMS.Domain.Enums;
using MMS.Domain.Security.Cryptography;

namespace MMS.Infrastructure.Migrations.Persistence;

[Migration(DatabaseVersions.CREATE_INITIAL_USER, "Creating initial user")]
public class InitialUser(
    IPasswordEncrypter encrypter
    ) : ForwardOnlyMigration
{
    private readonly IPasswordEncrypter _encrypter = encrypter;

    public override void Up()
    {
        Insert.IntoTable(TableNames.TABLE_COMPANIES)
            .Row(new
            {
                Id = 1,
                UpdatedOn = DateTime.UtcNow,
                CNPJ = "00000000000000",
                LegalName = "Delete-Me",
                CEP = "00000000",
                AddressNumber = "0000000000",
                PhoneNumber = "00000000000"
            });

        string hashedPassword = _encrypter.Encrypt("admin123456789");

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
