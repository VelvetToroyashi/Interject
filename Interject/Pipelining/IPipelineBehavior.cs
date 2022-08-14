using Interject.Contracts;

namespace Interject.Pipelining;

public delegate Task<TResponse> InerjectionContinuationDelegate<TResponse>();

public interface IPipelineBehavior<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, InerjectionContinuationDelegate<TResponse> next, CancellationToken cancellationToken);
}