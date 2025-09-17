using Microsoft.OpenApi.Models;
using Serilog;
using MMS.API.Filter;
using MMS.API.Middleware;
using MMS.API.Token;
using MMS.Application;
using MMS.Domain.Security.Token;
using MMS.Infrastructure;
using MMS.Infrastructure.Extensions;
using MMS.Infrastructure.Migrations;
//using MMS.Infrastructure.Workers;

var builder = WebApplication.CreateBuilder(args);

// Cannot be loaded in test
if (builder.Environment.EnvironmentName != "Test")
{
    // Configure serilog
    Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

    builder.Host.UseSerilog((context, service, configuration) =>
    {
        configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(service);

    });
}

// Add services to the container.
builder.Services.AddControllers();

// Add Cors Policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        string[]? allowedOrigins = builder.Configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>();
        if (allowedOrigins is not null && allowedOrigins.Length <= 0)
            return;

        policy.WithOrigins(allowedOrigins!)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(gen =>
{
    gen.OperationFilter<IdsFilter>();
    gen.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Meu Novo Sistema API",
        Version = "v1",
        Description = "Some"
    });
});

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();
builder.Services.AddScoped<IAccessTokenClaims, HttpContextLoggedUser>();
builder.Services.AddHttpContextAccessor();

//builder.Services.AddHostedService<MessageGrouper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(ui =>
    {
        ui.SwaggerEndpoint("/swagger/v1/swagger.json", "MMS v1");
    });
}

//app.MapGrpcService();

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors();

MigrationDatabase();

await app.RunAsync();


void MigrationDatabase()
{
    if (builder.Configuration.IsUnitTestEnvironment())
        return;

    string connectionString = builder.Configuration.ConnectionString();

    IServiceScope serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

    DatabaseMigration.Migrate(connectionString, serviceScope.ServiceProvider);
}

// Apenas para o sonar cloud
public partial class Program
{
    protected Program() { }
}
