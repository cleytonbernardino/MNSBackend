using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace MMS.Infrastructure.Migrations.Versions;

public abstract class VersionBase : ForwardOnlyMigration
{
    protected ICreateTableColumnOptionOrWithColumnSyntax CreateTable(string table)
    {
        return Create.Table(tableName: table)
            .WithColumn(name: "Id").AsInt64().PrimaryKey().Identity()
            .WithColumn(name: "Active").AsBoolean().NotNullable().WithDefaultValue(value: true)
            .WithColumn(name: "CreatedOn").AsDateTime().NotNullable().WithDefaultValue(value: DateTime.UtcNow);
    }
    
    protected ICreateTableColumnOptionOrWithColumnSyntax CreateTableShortId(string table)
    {
        return Create.Table(tableName: table)
            .WithColumn(name: "Id").AsInt16().PrimaryKey().Identity()
            .WithColumn(name: "Active").AsBoolean().NotNullable().WithDefaultValue(value: true)
            .WithColumn(name: "CreatedOn").AsDateTime().NotNullable().WithDefaultValue(value: DateTime.UtcNow);
    }
}
