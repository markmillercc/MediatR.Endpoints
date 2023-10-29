using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
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
    
    /// <summary>
    /// When an endpoint route is not explicitly defined, this function is used to generate the route from the <see cref="IRequest"/> or <see cref="IRequest{TResponse}"/> to which the endpoint is mapped
    /// By default this function removes all non-alphabetical characters from the full type name, then replaces dots with slashes.
    /// Example: Type "Api.Namespace.ClassName" creates route "Api/Namespace/ClassName"
    /// </summary>
    public Func<Type, string> DefaultRouteFormatter { get; set; } = (t) => Regex.Replace(t.FullName, "[^a-zA-Z.]+", "").Replace(".", "/");

    /// <summary>
    /// Adds additional assemblies to scan for endpoint configurations. Assemblies are also pulled from MediatR registration. 
    /// This call is only necessary if you have endpoints configured in assemblies not registered via AddMediatR.
    /// </summary>
    /// <param name="assemblies">Assemblies to scan</param>
    /// <returns>This configuration instance</returns>
    public MediatREndpointsConfiguration RegisterFromAssemblies(params Assembly[] assemblies)
    {
        Assemblies = Assemblies.Concat(assemblies).Distinct().ToArray();
        return this;
    }

    /// <summary>
    /// Overrides <see cref="DefaultEndpointHandlerDelegateFactory"/>
    /// </summary>
    /// <typeparam name="T">The <see cref="IEndpointHandlerDelegateFactory"/> type</typeparam>
    /// <returns>This configuration instance</returns>
    public MediatREndpointsConfiguration UseEndpointHandlerDelegateFactory<T>() where T : IEndpointHandlerDelegateFactory, new()
    {
        return UseEndpointHandlerDelegateFactory(new T());
    }

    /// <summary>
    /// Overrides <see cref="DefaultEndpointHandlerDelegateFactory"/>
    /// </summary>
    /// <param name="factory">The <see cref="IEndpointHandlerDelegateFactory"/> instance</param>
    /// <returns>This configuration instance</returns>
    public MediatREndpointsConfiguration UseEndpointHandlerDelegateFactory(IEndpointHandlerDelegateFactory factory)
    {
        EndpointHandlerDelegateFactory = factory;
        return this;
    }

    /// <summary>
    /// Adds global convention(s) which applies to all endpoints. Subsequent calls will add to existing conventions, not override them.
    /// </summary>
    /// <param name="action">The action that defines your custom convention(s)</param>
    /// <returns></returns>
    public MediatREndpointsConfiguration AddRouteGroupBuilder(Action<RouteGroupBuilder> action)
    {
        GlobalRouteGroupBuilder += action;
        return this;
    }

    /// <summary>
    /// Adds convention(s) to all endpoints in the specified group.  Subsequent calls will add to existing conventions, not override them.
    /// </summary>
    /// <param name="group">The name of the group to apply convention(s) to</param>
    /// <param name="action">The action that defines your custom convention(s)</param>
    /// <returns></returns>
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

    /// <summary>
    /// Registers an endpoint
    /// </summary>
    /// <typeparam name="TRequest">The <see cref="IRequest"/> or <see cref="IRequest{TResponse}"/> object this endpoints refers to</typeparam>
    /// <param name="method">The HTTP Method. GET, POST, PATCH, PUT, or DELETE are supported</param>
    /// <param name="route">The endpoint route pattern</param>
    /// <param name="group">The endpoint group. Prefixed to route and used for applying custom conventions to groups of endpoints</param>
    /// <param name="action">Custom conventions to apply to this endpoint</param>
    /// <param name="handler">Custom handler for this endpoint</param>
    /// <returns>This configuration instance</returns>
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

    /// <summary>
    /// Registers an endpoint using HTTP Method POST
    /// </summary>
    /// <typeparam name="TRequest">The <see cref="IRequest"/> or <see cref="IRequest{TResponse}"/> object this endpoints refers to</typeparam>
    /// <param name="route">Optional route pattern. If omitted, route is generated using the <see cref="DefaultRouteFormatter"/></param>
    /// <param name="group">Optional group. If omitted, an empty string is used</param>
    /// <param name="builder">Optionally apply custom conventions to this endpoint</param>
    /// <param name="handler">Optionally apply a custom handler to this endpoint</param>
    /// <returns>This configuration instance</returns>
    public MediatREndpointsConfiguration MapPost<TRequest>(string route = default, string group = "", Action<RouteHandlerBuilder> builder = default, Delegate handler = default)
        where TRequest : IBaseRequest
        => Map<TRequest>(Method.Post, route, group, builder, handler);

    /// <summary>
    /// Registers an endpoint using HTTP Method GET
    /// </summary>
    /// <typeparam name="TRequest">The <see cref="IRequest"/> or <see cref="IRequest{TResponse}"/> object this endpoints refers to</typeparam>
    /// <param name="route">Optional route pattern. If omitted, route is generated using the <see cref="DefaultRouteFormatter"/></param>
    /// <param name="group">Optional group. If omitted, an empty string is used</param>
    /// <param name="builder">Optionally apply custom conventions to this endpoint</param>
    /// <param name="handler">Optionally apply a custom handler to this endpoint</param>
    /// <returns>This configuration instance</returns>
    public MediatREndpointsConfiguration MapGet<TRequest>(string route = default, string group = "", Action<RouteHandlerBuilder> builder = default, Delegate handler = default)
        where TRequest : IBaseRequest
        => Map<TRequest>(Method.Get, route, group, builder, handler);

    /// <summary>
    /// Registers an endpoint using HTTP Method PUT
    /// </summary>
    /// <typeparam name="TRequest">The <see cref="IRequest"/> or <see cref="IRequest{TResponse}"/> object this endpoints refers to</typeparam>
    /// <param name="route">Optional route pattern. If omitted, route is generated using the <see cref="DefaultRouteFormatter"/></param>
    /// <param name="group">Optional group. If omitted, an empty string is used</param>
    /// <param name="builder">Optionally apply custom conventions to this endpoint</param>
    /// <param name="handler">Optionally apply a custom handler to this endpoint</param>
    /// <returns>This configuration instance</returns>
    public MediatREndpointsConfiguration MapPut<TRequest>(string route = default, string group = "", Action<RouteHandlerBuilder> builder = default, Delegate handler = default)
        where TRequest : IBaseRequest
        => Map<TRequest>(Method.Put, route, group, builder, handler);

    /// <summary>
    /// Registers an endpoint using HTTP Method PATCH
    /// </summary>
    /// <typeparam name="TRequest">The <see cref="IRequest"/> or <see cref="IRequest{TResponse}"/> object this endpoints refers to</typeparam>
    /// <param name="route">Optional route pattern. If omitted, route is generated using the <see cref="DefaultRouteFormatter"/></param>
    /// <param name="group">Optional group. If omitted, an empty string is used</param>
    /// <param name="builder">Optionally apply custom conventions to this endpoint</param>
    /// <param name="handler">Optionally apply a custom handler to this endpoint</param>
    /// <returns>This configuration instance</returns>
    public MediatREndpointsConfiguration MapPatch<TRequest>(string route = default, string group = "", Action<RouteHandlerBuilder> builder = default, Delegate handler = default)
        where TRequest : IBaseRequest
        => Map<TRequest>(Method.Patch, route, group, builder, handler);

    /// <summary>
    /// Registers an endpoint using HTTP Method DELETE
    /// </summary>
    /// <typeparam name="TRequest">The <see cref="IRequest"/> or <see cref="IRequest{TResponse}"/> object this endpoints refers to</typeparam>
    /// <param name="route">Optional route pattern. If omitted, route is generated using the <see cref="DefaultRouteFormatter"/></param>
    /// <param name="group">Optional group. If omitted, an empty string is used</param>
    /// <param name="builder">Optionally apply custom conventions to this endpoint</param>
    /// <param name="handler">Optionally apply a custom handler to this endpoint</param>
    /// <returns>This configuration instance</returns>
    public MediatREndpointsConfiguration MapDelete<TRequest>(string route = default, string group = "", Action<RouteHandlerBuilder> builder = default, Delegate handler = default)
        where TRequest : IBaseRequest
        => Map<TRequest>(Method.Delete, route, group, builder, handler);
}


