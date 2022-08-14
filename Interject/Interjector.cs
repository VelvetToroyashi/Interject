using Interject.Contracts;
using Interject.Pipelining;
using Microsoft.Extensions.DependencyInjection;

namespace Interject;

public class Interjector : IInterjector
{
    private readonly IServiceProvider _services;
    
    public Interjector(IServiceProvider services)
    {
        _services = services;
    }
    
    public virtual async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken ct = default)
    {
        await using var scope = _services.CreateAsyncScope(); // This is *vital*. Creating a scope prevents sticky dependencies.
        var services = scope.ServiceProvider;
        
        var handler = services.GetRequiredService<IRequestHandler<IRequest<TResponse>, TResponse>>();

        Task<TResponse> HandleAsync() => services.GetRequiredService<IRequestHandler<IRequest<TResponse>, TResponse>>().Handle(request, ct);
        
        var pipelines = services
            .GetServices<IPipelineBehavior<IRequest<TResponse>, TResponse>>()
            .Reverse()
            .Aggregate((InerjectionContinuationDelegate<TResponse>)HandleAsync, (next, behavior) => () => behavior.Handle(request, next, ct));
        
        return await pipelines();
    }
}