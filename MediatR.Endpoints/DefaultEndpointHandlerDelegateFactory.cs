using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediatR.Endpoints;

/// <summary>
/// Defines default GET, POST, PUT, PATCH, DELETE action delegates for requests with and without responses.
/// In each case, <see cref="IMediator"/> is injected to send the given request and, if applicable, the response is returned. 
/// Parameter binding varies per HTTP method. Virtual methods to allow custom handling and/or parameter binding per endpoint variation.
/// </summary>
public class DefaultEndpointHandlerDelegateFactory : IEndpointHandlerDelegateFactory
{
    public Delegate GetEndpointHandlerDelegate<TRequest>(Method httpMethod) where TRequest : IRequest
        => httpMethod switch
        {
            Method.Get => GetHandler<TRequest>(),
            Method.Delete => DeleteHandler<TRequest>(),
            Method.Post => PostHandler<TRequest>(),
            Method.Patch => PatchHandler<TRequest>(),
            Method.Put => PutHandler<TRequest>(),
            _ => throw new NotSupportedException(),
        };

    public Delegate GetEndpointHandlerDelegate<TRequest, TResponse>(Method httpMethod) where TRequest : IRequest<TResponse>
        => httpMethod switch
        {
            Method.Get => GetHandler<TRequest, TResponse>(),
            Method.Delete => DeleteHandler<TRequest, TResponse>(),
            Method.Post => PostHandler<TRequest, TResponse>(),
            Method.Patch => PatchHandler<TRequest, TResponse>(),
            Method.Put => PutHandler<TRequest, TResponse>(),
            _ => throw new NotSupportedException(),
        };

    public virtual Delegate GetHandler<TRequest>() where TRequest : IRequest => 
        async (IMediator mediator, [AsParameters] TRequest request) => 
            await mediator.Send(request);
        
    public virtual Delegate GetHandler<TRequest, TResponse>() where TRequest : IRequest<TResponse> => 
        async (IMediator mediator, [AsParameters] TRequest request) => 
            await mediator.Send(request);
    
    public virtual Delegate DeleteHandler<TRequest>() where TRequest : IRequest => 
        async (IMediator mediator, [AsParameters] TRequest request) => 
            await mediator.Send(request);  

    public virtual Delegate DeleteHandler<TRequest, TResponse>() where TRequest : IRequest<TResponse> => 
        async (IMediator mediator, [AsParameters] TRequest request) =>
            await mediator.Send(request);

    public virtual Delegate PatchHandler<TRequest>() where TRequest : IRequest =>
        async (IMediator mediator, [FromBody] TRequest request) =>
            await mediator.Send(request);

    public virtual Delegate PatchHandler<TRequest, TResponse>() where TRequest : IRequest<TResponse> => 
        async (IMediator mediator, [FromBody] TRequest request) => 
            await mediator.Send(request);

    public virtual Delegate PostHandler<TRequest>() where TRequest : IRequest => 
        async (IMediator mediator, [FromBody] TRequest request) => 
            await mediator.Send(request);

    public virtual Delegate PostHandler<TRequest, TResponse>() where TRequest : IRequest<TResponse> => 
        async (IMediator mediator, [FromBody] TRequest request) => 
            await mediator.Send(request);

    public virtual Delegate PutHandler<TRequest>() where TRequest : IRequest => 
        async (IMediator mediator, [FromBody] TRequest request) => 
            await mediator.Send(request);        

    public virtual Delegate PutHandler<TRequest, TResponse>() where TRequest : IRequest<TResponse> => 
        async (IMediator mediator, [FromBody] TRequest request) => 
            await mediator.Send(request);
}

