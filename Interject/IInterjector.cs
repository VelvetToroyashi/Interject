using Interject.Contracts;

namespace Interject;

public interface IInterjector
{
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken ct = default);
}