namespace MediatR.Endpoints;

/// <summary>
/// Registers an endpoint
/// </summary>
/// <typeparam name="TRequest">The <see cref="IRequest"/> or <see cref="IRequest{TResponse}"/> object to which this endpoint maps</typeparam>
public interface IEndpointConfiguration<TRequest> : IBaseEndpointConfiguration
    where TRequest : IBaseRequest
{ 
} 

/// <summary>
/// Base class should not be used directly. Instead use <see cref="IEndpointConfiguration{TRequest}"/>
/// </summary>
public interface IBaseEndpointConfiguration
{
    /// <summary>
    /// Add configuration details for your endpoint
    /// </summary>
    /// <param name="e">The object that defines your endpoint</param>
    public void Configure(IEndpointDefinition e);
}

