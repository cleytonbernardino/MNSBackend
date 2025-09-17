using FluentMigrator;

namespace MMS.Infrastructure.Migrations.Versions;

[Migration(
    DatabaseVersions.TABLE_COMPANY, 
    "Creating the companies table and a table for a 1:1 relationship to control payments and plans."
    )]
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
            .WithColumn("SubscriptionStatus").AsBoolean().NotNullable().WithDefaultValue(value: false)
            .WithColumn("Website").AsString().Nullable();
        
        CreateTable(TableNames.TABLE_COMPANY_SUBSCRIPTION)
            .WithColumn("SubscriptionPlanId").AsInt16().Nullable()
                .ForeignKey("fk_subscriptions_plans_id", TableNames.TABLE_SUBSCRIPTIONS_PLANS, "Id")
                .OnDelete(System.Data.Rule.SetNull)
            .WithColumn("IsBillingAnnual").AsBoolean().NotNullable().WithDefaultValue(value: false)
            .WithColumn("PaymentStatus").AsInt16().NotNullable()
            .WithColumn("NextBillingDate").AsDateTime().NotNullable()
            .WithColumn("PaymentMethod").AsInt16().NotNullable();
    }
}
