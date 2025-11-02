using FluentMigrator;

namespace MMS.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_SUBSCRIPTIONS, "Creating the subscriptions table")]
public class Version0000001 : VersionBase
{
    public override void Up()
    {
        CreateTableShortId(TableNames.TABLE_SUBSCRIPTIONS_PLANS)
            .WithColumn("Name").AsString(30).NotNullable()
            .WithColumn("Description").AsString(100).Nullable()
            .WithColumn("Properties").AsString(510).NotNullable()
            .WithColumn("Price").AsDouble().NotNullable();
    }
}
