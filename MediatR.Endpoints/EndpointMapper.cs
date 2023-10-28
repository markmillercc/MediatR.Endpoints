using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System.Reflection;

namespace MediatR.Endpoints;

internal static class EndpointMapper
{
    public static RouteHandlerBuilder MapEndpoint(RouteGroupBuilder group, EndpointRegistration registration, IEndpointHandlerDelegateFactory handlerFactory)
    {
        var bindingAttributes = BindingFlags.NonPublic | BindingFlags.Static;
        var methodArgTypes = new Type[] { typeof(IEndpointRouteBuilder), typeof(EndpointRegistration), typeof(IEndpointHandlerDelegateFactory) };

        MethodInfo mappingMethod;
        if (registration.ResponseType == null)
        {
            mappingMethod = typeof(EndpointMapper)
                .GetMethod(nameof(MapRequest), bindingAttributes, methodArgTypes)
                .MakeGenericMethod(registration.RequestType);
        }
        else
        {
            mappingMethod = typeof(EndpointMapper)
                .GetMethod(nameof(MapRequestReponse), bindingAttributes, methodArgTypes)
                .MakeGenericMethod(registration.RequestType, registration.ResponseType);
        }

        var builder = (RouteHandlerBuilder)mappingMethod.Invoke(null, new object[] { group, registration, handlerFactory });
        registration.RouteHandlerBuilder?.Invoke(builder);

        return builder;
    }


    private static RouteHandlerBuilder MapRequestReponse<TRequest, TResponse>(IEndpointRouteBuilder root, EndpointRegistration registration, IEndpointHandlerDelegateFactory handlerFactory)
         where TRequest : IRequest<TResponse>
    {
        var handler = registration.Handler 
            ?? handlerFactory.GetEndpointHandlerDelegate<TRequest, TResponse>(registration.HttpMethod);

        return Map(root, registration.HttpMethod, registration.Route, handler);
    }

    private static RouteHandlerBuilder MapRequest<TRequest>(IEndpointRouteBuilder root, EndpointRegistration registration, IEndpointHandlerDelegateFactory handlerFactory)
         where TRequest : IRequest
    {
        var handler = registration.Handler
            ?? handlerFactory.GetEndpointHandlerDelegate<TRequest>(registration.HttpMethod);

        return Map(root, registration.HttpMethod, registration.Route, handler);
    }

    private static RouteHandlerBuilder Map(IEndpointRouteBuilder root, Method httpMethod, string route, Delegate handler)
    {
        return httpMethod switch
        {
            Method.Get => root.MapGet(route, handler),
            Method.Post => root.MapPost(route, handler),
            Method.Patch => root.MapPatch(route, handler),
            Method.Put => root.MapPut(route, handler),
            Method.Delete => root.MapDelete(route, handler),
            _ => throw new NotSupportedException($"Unsupported HTTP Method: {httpMethod}"),
        };
    }
}

