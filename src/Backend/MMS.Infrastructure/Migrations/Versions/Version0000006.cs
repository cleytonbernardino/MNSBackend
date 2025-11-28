using FluentMigrator;
using System.Data;

namespace MMS.Infrastructure.Migrations.Versions;

[Migration(
    DatabaseVersions.TABLE_SERVICES, 
    "Creating the table for registering services to be provided.")]
public class Version0000006 : VersionBase
{
    public override void Up()
    {
        CreateTable(TableNames.TABLE_SERVICES)
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("Description").AsString().Nullable()
            .WithColumn("ServiceType").AsString().NotNullable()
            .WithColumn("Status").AsInt16().Nullable()
            .WithColumn("CompanyId").AsInt64().NotNullable()
                .ForeignKey("fk_services_company_id", TableNames.TABLE_COMPANIES, "Id")
                .OnDelete(Rule.Cascade)
            .WithColumn("RegisteredBy").AsGuid().NotNullable()
                .ForeignKey("fk_users_id", TableNames.TABLE_USER, "Id")
                .OnDelete(Rule.Cascade);
    }
}
