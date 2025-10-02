using Microsoft.Extensions.Configuration;

namespace InzRate.Core.Infrastructure.Extensions;

public static class ServiceProviderExtensions
{
    public static void InitializeServices(this IServiceProvider services, IConfiguration configuration)
    {
        // Init any service from here
    }
}