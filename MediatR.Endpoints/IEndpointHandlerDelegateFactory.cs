namespace MediatR.Endpoints;

public interface IEndpointHandlerDelegateFactory
{
    public Delegate GetEndpointHandlerDelegate<TRequest>(Method httpMethod) where TRequest : IRequest;
    public Delegate GetEndpointHandlerDelegate<TRequest, TResponse>(Method httpMethod) where TRequest : IRequest<TResponse>;
}

