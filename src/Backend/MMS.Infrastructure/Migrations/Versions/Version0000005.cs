using FluentMigrator;
using System.Data;

namespace MMS.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_REFRESH_TOKENS, "Creating a table to store refreshTokens")]
public class Version0000005 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table(TableNames.TABLE_REFRESH_TOKEN)
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("UserIdentifier").AsGuid().NotNullable()
                .ForeignKey("fk_users_user_identifier", TableNames.TABLE_USER, "UserIdentifier")
                .OnDelete(Rule.Cascade)
            .WithColumn("Token").AsString().Nullable()
            .WithColumn("ExpiryDate").AsDateTime().Nullable();
    }
}
