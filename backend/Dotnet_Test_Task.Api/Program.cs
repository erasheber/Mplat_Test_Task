using Dotnet_Test_Task.Api.Security;
using Dotnet_Test_Task.Application;
using Dotnet_Test_Task.Infrastructure;
using Dotnet_Test_Task.Infrastructure.Persistence;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using NSwag;
using NSwag.Generation.Processors.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiKeyOptions>(builder.Configuration.GetSection("Security"));

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.Title = "Dotnet_Test_Task API";

        s.AddSecurity("ApiKey", [], new OpenApiSecurityScheme
        {
            Type = OpenApiSecuritySchemeType.ApiKey,
            Name = "X-Api-Key",
            In = OpenApiSecurityApiKeyLocation.Header,
            Description = "API key needed to access endpoints"
        });

        s.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("ApiKey"));
    };
});

var app = builder.Build();

if (app.Configuration.GetValue<bool>("Database:AutoMigrate"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<PaymentsDbContext>();
    db.Database.Migrate();
}

app.UseMiddleware<ApiKeyMiddleware>();

app.UseFastEndpoints();
app.UseSwaggerGen();

app.Run();
