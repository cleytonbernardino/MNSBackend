using FluentMigrator;

namespace MMS.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_COMPANY, "Creating the company table")]
public class Version0000002 : VersionBase
{
    public override void Up()
    {
        CreateTable(TableNames.TABLE_COMPANIES)
            .WithColumn("UpdatedOn").AsDateTime().NotNullable()
            .WithColumn("CNPJ").AsString(14).NotNullable()
            .WithColumn("LegalName").AsString(100).NotNullable()
            .WithColumn("DoingBusinessAs").AsString(100).Nullable()
            .WithColumn("BusinessSector").AsString(50).Nullable()
            .WithColumn("CEP").AsString(8).NotNullable()
            .WithColumn("AddressNumber").AsString(10).NotNullable()
            .WithColumn("BusinessEmail").AsString().Nullable()
            .WithColumn("PhoneNumber").AsString(13).NotNullable()
            .WithColumn("WhatsappAPINumber").AsString(20).Nullable()
            .WithColumn("SubscriptionPlan").AsInt64().Nullable().ForeignKey("fk_company_subscription_id", TableNames.TABLE_COMPANY_SUBSCRIPTION, "Id")
            .WithColumn("SubscriptionStatus").AsBoolean().NotNullable().WithDefaultValue(value: false)
            .WithColumn("Website").AsString().Nullable();
    }
}
