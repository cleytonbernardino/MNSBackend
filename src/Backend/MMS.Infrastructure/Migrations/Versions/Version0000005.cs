using FluentMigrator;

namespace MMS.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.COMPANY_MANGER_ID, "Adding company manager ID")]
public class Version0000005 : VersionBase
{
    public override void Up()
    {
        Alter.Table(TableNames.TABLE_COMPANIES)
            .AddColumn("ManagerId").AsInt64().Nullable().ForeignKey("fk_user_id", "users", "Id");
    }
}
