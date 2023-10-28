using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediatR.Endpoints;

public class DefaultEndpointHandlerDelegateFactory : IEndpointHandlerDelegateFactory
{    
    public virtual Delegate GetHandler<TRequest>()
        where TRequest : IRequest
    {
        return async (IMediator mediator, [AsParameters] TRequest request) =>
        {
            await mediator.Send(request);
            return Results.Ok();
        };
    }

    public virtual Delegate GetHandler<TRequest, TResponse>()
        where TRequest : IRequest<TResponse>
    {
        return async (IMediator mediator, [AsParameters] TRequest request) =>
        {
            var response = await mediator.Send(request);
            return Results.Ok(response);
        };
    }

    public virtual Delegate DeleteHandler<TRequest>()
        where TRequest : IRequest
    {
        return async (IMediator mediator, [AsParameters] TRequest request) =>
        {
            await mediator.Send(request);
            return Results.Ok();
        };
    }    

    public virtual Delegate DeleteHandler<TRequest, TResponse>()
        where TRequest : IRequest<TResponse>
    {
        return async (IMediator mediator, [AsParameters] TRequest request) =>
        {
            var response = await mediator.Send(request);
            return Results.Ok(response);
        };
    }

    public virtual Delegate PatchHandler<TRequest>()
        where TRequest : IRequest
    {
        return async (IMediator mediator, [FromBody] TRequest request) =>
        {
            await mediator.Send(request);
            return Results.Ok();
        };
    }

    public virtual Delegate PatchHandler<TRequest, TResponse>()
        where TRequest : IRequest<TResponse>
    {
        return async (IMediator mediator, [FromBody] TRequest request) =>
        {
            var response = await mediator.Send(request);
            return Results.Ok(response);
        };
    }

    public virtual Delegate PostHandler<TRequest>()
        where TRequest : IRequest
    {
        return async (IMediator mediator, [FromBody] TRequest request) =>
        {
            await mediator.Send(request);
            return Results.Ok();
        };
    }

    public virtual Delegate PostHandler<TRequest, TResponse>()
        where TRequest : IRequest<TResponse>
    {
        return async (IMediator mediator, [FromBody] TRequest request) =>
        {
            var response = await mediator.Send(request);
            return Results.Ok(response);
        };
    }

    public virtual Delegate PutHandler<TRequest>()
        where TRequest : IRequest
    {
        return async (IMediator mediator, [FromBody] TRequest request) =>
        {
            await mediator.Send(request);
            return Results.Ok();
        };
    }        

    public virtual Delegate PutHandler<TRequest, TResponse>()
        where TRequest : IRequest<TResponse>
    {
        return async (IMediator mediator, [FromBody] TRequest request) =>
        {
            var response = await mediator.Send(request);
            return Results.Ok(response);
        };
    }

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
}

