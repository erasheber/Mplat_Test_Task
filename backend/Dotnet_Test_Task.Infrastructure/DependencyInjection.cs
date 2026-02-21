using Dotnet_Test_Task.Application.Interfaces;
using Dotnet_Test_Task.Infrastructure.Persistence;
using Dotnet_Test_Task.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet_Test_Task.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddDbContext<PaymentsDbContext>(opt =>
            opt.UseNpgsql(cfg.GetConnectionString("Default")));

        services.AddScoped<IPaymentsRepository, PaymentsRepository>();

        return services;
    }
}