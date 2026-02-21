using Dotnet_Test_Task.Application.Interfaces;
using Dotnet_Test_Task.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet_Test_Task.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IPaymentsService, PaymentsService>();
        return services;
    }
}
