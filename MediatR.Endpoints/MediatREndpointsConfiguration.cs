using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MediatR.Endpoints;

public class MediatREndpointsConfiguration
{    
    internal Assembly[] Assemblies { get; set; } = Array.Empty<Assembly>();
    internal List<EndpointRegistration> Registrations { get; set; } = new List<EndpointRegistration>();
    internal IEndpointHandlerDelegateFactory EndpointHandlerDelegateFactory { get; set; } = new DefaultEndpointHandlerDelegateFactory();
    internal IDictionary<string, Action<RouteGroupBuilder>> RouteGroupBuilders { get; set; } = new Dictionary<string, Action<RouteGroupBuilder>>();
    internal Action<RouteGroupBuilder> GlobalRouteGroupBuilder { get; set; } = a => { };
    public Func<Type, string> DefaultRouteFormatter { get; set; } = (t) => Regex.Replace(t.FullName, "[^a-zA-Z.]+", "").Replace(".", "/");

    public MediatREndpointsConfiguration RegisterFromAssemblies(params Assembly[] assemblies)
    {
        Assemblies = Assemblies.Concat(assemblies).Distinct().ToArray();
        return this;
    }

    public MediatREndpointsConfiguration UseEndpointHandlerDelegateFactory<T>() where T : IEndpointHandlerDelegateFactory, new()
    {
        return UseEndpointHandlerDelegateFactory(new T());
    }

    public MediatREndpointsConfiguration UseEndpointHandlerDelegateFactory(IEndpointHandlerDelegateFactory factory)
    {
        EndpointHandlerDelegateFactory = factory;
        return this;
    }

    public MediatREndpointsConfiguration AddRouteGroupBuilder(Action<RouteGroupBuilder> action)
    {
        GlobalRouteGroupBuilder += action;
        return this;
    }

    public MediatREndpointsConfiguration AddRouteGroupBuilder(string group, Action<RouteGroupBuilder> action)
    {
        if (RouteGroupBuilders.TryGetValue(group, out var builder))
        {
            builder += action;
            RouteGroupBuilders[group] = builder;
        }
        else
        {
            RouteGroupBuilders.Add(group, action);
        }
        return this;
    }

    public MediatREndpointsConfiguration Map<TRequest>(Method method, string route, string group, Action<RouteHandlerBuilder> action, Delegate handler)
        where TRequest : IBaseRequest
    {
        Registrations.Add(new EndpointRegistration(typeof(TRequest))
        {
            HttpMethod = method,
            Route = route,
            Group = group,            
            RouteHandlerBuilder = action,
            Handler = handler
        });
        return this;
    }

    public MediatREndpointsConfiguration MapPost<TRequest>(string route = default, string group = "", Action<RouteHandlerBuilder> builder = default, Delegate handler = default)
        where TRequest : IBaseRequest
        => Map<TRequest>(Method.Post, route, group, builder, handler);

    public MediatREndpointsConfiguration MapGet<TRequest>(string route = default, string group = "", Action<RouteHandlerBuilder> builder = default, Delegate handler = default)
        where TRequest : IBaseRequest
        => Map<TRequest>(Method.Get, route, group, builder, handler);

    public MediatREndpointsConfiguration MapPut<TRequest>(string route = default, string group = "", Action<RouteHandlerBuilder> builder = default, Delegate handler = default)
        where TRequest : IBaseRequest
        => Map<TRequest>(Method.Put, route, group, builder, handler);

    public MediatREndpointsConfiguration MapPatch<TRequest>(string route = default, string group = "", Action<RouteHandlerBuilder> builder = default, Delegate handler = default)
        where TRequest : IBaseRequest
        => Map<TRequest>(Method.Patch, route, group, builder, handler);

    public MediatREndpointsConfiguration MapDelete<TRequest>(string route = default, string group = "", Action<RouteHandlerBuilder> builder = default, Delegate handler = default)
        where TRequest : IBaseRequest
        => Map<TRequest>(Method.Delete, route, group, builder, handler);
}


