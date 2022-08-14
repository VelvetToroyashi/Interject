using System.Reflection;
using Interject.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Interject.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInterject(this ServiceCollection services, ServiceLifetime lifetime, params Assembly[] assemblies)
    {
        services.TryAdd(ServiceDescriptor.Describe(typeof(IInterjector), typeof(Interjector), lifetime));

        var handlers = assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(dt => dt.ImplementedInterfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)));
        
        foreach (var handler in handlers)
            services.TryAdd(ServiceDescriptor.Describe(typeof(IRequestHandler<,>), handler, lifetime));

        return services;
    }
    
}