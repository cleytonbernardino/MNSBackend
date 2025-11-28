using FluentMigrator;

namespace MMS.Infrastructure.Migrations.Persistence;

[Migration(DatabaseVersions.REGISTER_INITIAL_COMPANY, "Creating initial company")]
public class RegisterInitialCompany : ForwardOnlyMigration
{
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
                Address = "Rua One",
                AddressNumber = "0000000000",
                PhoneNumber = "00000000000"
            });
    }
}
