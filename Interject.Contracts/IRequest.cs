namespace Interject.Contracts;

public interface IRequest : IRequest<Unit> { }

public interface IRequest<out T> { }

public readonly record struct Unit
{
    private static readonly Unit _ref = new();
    
    public static Unit Value => _ref;
}