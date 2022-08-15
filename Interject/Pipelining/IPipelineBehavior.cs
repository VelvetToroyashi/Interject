using Interject.Contracts;

namespace Interject.Pipelining;

public delegate Task<TResponse> InterjectionContinuationDelegate<TResponse>();

public interface IPipelineBehavior<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, InterjectionContinuationDelegate<TResponse> next, CancellationToken cancellationToken);
}