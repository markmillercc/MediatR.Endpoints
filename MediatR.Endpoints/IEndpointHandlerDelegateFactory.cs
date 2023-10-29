namespace MediatR.Endpoints;

/// <summary>
/// When building endpoints, these methods are used for determining the appropriate action delegate to apply given the specified request and the HTTP method.
/// Defaults can be found in <see cref="DefaultEndpointHandlerDelegateFactory"/>. To modify behavior, create a custom factory and configure during service registration via
/// <see cref="MediatREndpointsConfiguration.UseEndpointHandlerDelegateFactory"/>
/// </summary>
public interface IEndpointHandlerDelegateFactory
{
    /// <summary>
    /// For requests without a response
    /// </summary>
    /// <typeparam name="TRequest">The request to which this endpoint is mapped</typeparam>
    /// <param name="httpMethod">The HTTP method to use</param>
    /// <returns>The delegate action to perform given the specified request type and HTTP method</returns>
    public Delegate GetEndpointHandlerDelegate<TRequest>(Method httpMethod) where TRequest : IRequest;

    /// <summary>
    /// For requests with a response
    /// </summary>
    /// <typeparam name="TRequest">The request to which this endpoint is mapped</typeparam>
    /// <typeparam name="TResponse">The request response type</typeparam>
    /// <param name="httpMethod">The requested HTTP method</param>
    /// <returns>The delegate action to perform given the specified request type and HTTP method</returns>
    public Delegate GetEndpointHandlerDelegate<TRequest, TResponse>(Method httpMethod) where TRequest : IRequest<TResponse>;
}

