using FluentMigrator;
using System.Data;

namespace MMS.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.Add_Foreign_Keys, "Adding relationships to the company table")]
public class Version0000004 : VersionBase
{
    public override void Up()
    {
        Alter.Table(TableNames.TABLE_COMPANIES)
            .AddColumn("ManagerId").AsInt64().Nullable()
                .ForeignKey("fk_user_id", TableNames.TABLE_USER, "Id")
                .OnDelete(Rule.SetNull);

        Alter.Table(TableNames.TABLE_COMPANY_SUBSCRIPTION)
            .AddColumn("CompanyId").AsInt64().Unique().NotNullable()
                .ForeignKey("fk_company_id", TableNames.TABLE_COMPANIES, "Id")
                .OnDelete(Rule.Cascade);
    }
}
