using Dotnet_Test_Task.Api.Security;
using Dotnet_Test_Task.Infrastructure.Persistence;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiKeyOptions>(builder.Configuration.GetSection("Security"));

builder.Services.AddDbContext<PaymentsDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

var app = builder.Build();

app.UseMiddleware<ApiKeyMiddleware>();

app.UseFastEndpoints();
app.UseSwaggerGen();

app.Run();
