using FluentMigrator;

namespace MMS.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_USER, "Creating the first user model")]
public class Version0000003 : VersionBase
{
    public override void Up()
    {
        CreateTable(TableNames.TABLE_USER)
            .WithColumn("IsAdmin").AsBoolean().WithDefaultValue(value: false)
            .WithColumn("UpdatedOn").AsDateTime().NotNullable()
            .WithColumn("LastLogin").AsDateTime().NotNullable()
            .WithColumn("UserIdentifier").AsGuid().NotNullable().Unique()
            .WithColumn("Email").AsString().NotNullable().Unique()
            .WithColumn("Phone").AsString(13).NotNullable()
            .WithColumn("FirstName").AsString(40).NotNullable()
            .WithColumn("LastName").AsString(50).Nullable()
            .WithColumn("Password").AsString().NotNullable()
            .WithColumn("Role").AsInt16().NotNullable()
            .WithColumn("CompanyId").AsInt64().NotNullable()
                .ForeignKey("fk_companies_id", TableNames.TABLE_COMPANIES, "Id")
                .OnDelete(System.Data.Rule.Cascade);
    }
}
