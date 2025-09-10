using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace MMS.Infrastructure.Migrations;

public static class DatabaseMigration
{
    public static void Migrate(string connectionString, IServiceProvider service)
    {
        EnsureDatabaseCreated(connectionString);
        MigrateDatabase(service);
    }

    // Mysql
    private static void EnsureDatabaseCreated(string connectionString)
    {
        MySqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
        string dataBaseName = connectionStringBuilder.Database;
        connectionStringBuilder.Remove("Database");

        using MySqlConnection dbConnection = new(connectionStringBuilder.ConnectionString);
        using MySqlCommand command = new($"create database if not exists {dataBaseName};", dbConnection);

        dbConnection.Open();
        command.ExecuteNonQuery();
        dbConnection.Close();
    }

    private static void MigrateDatabase(IServiceProvider service)
    {
        IMigrationRunner runner = service.GetRequiredService<IMigrationRunner>();
        runner.ListMigrations();
        runner.MigrateUp();
    }
}
