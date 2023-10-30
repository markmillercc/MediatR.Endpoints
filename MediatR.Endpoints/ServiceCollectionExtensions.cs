using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace MediatR.Endpoints;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Uses assemblies found during MediatR registration to scan for and register endpoint configurations
    /// </summary>
    /// <param name="services">Your service collection</param>
    /// <param name="configAction">Action used to configure endpoints and endpoint options</param>
    /// <returns>Your service collection</returns>
    /// <exception cref="InvalidOperationException">Throws if <see cref="IMediator"/> has not been registered</exception>
    public static IServiceCollection AddMediatREndoints(this IServiceCollection services, Action<MediatREndpointsConfiguration> configAction = null)
    {
        var config = new MediatREndpointsConfiguration();

        if (!services.Any(a => a.ServiceType == typeof(IMediator)))
            throw new InvalidOperationException("Missing IMediator service. Did you forget to call AddMediatR?");

        var assemblies = services
            .Where(a => a.ServiceType.IsGenericType)
            .Where(a => a.ServiceType.GetGenericTypeDefinition() == typeof(IRequestHandler<>)
                || a.ServiceType.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
            .Select(a => a.ImplementationType.Assembly)
            .Distinct()
            .ToArray();

        configAction?.Invoke(config);
        config.RegisterFromAssemblies(assemblies);
        Registrar.Initialize(config);
        return services;
    }

    /// <summary>
    /// Finds endpoints configured via <see cref="EndpointAttribute"/> or <see cref="IEndpointConfiguration{TRequest}"/>, or during service configuration, and maps as defined to your application 
    /// </summary>
    /// <param name="app">Your web application</param>
    /// <exception cref="InvalidOperationException">Throws if <see cref="AddMediatREndoints(IServiceCollection, Action{MediatREndpointsConfiguration})"/> has not been called yet</exception>
    public static void MapMediatREndpoints(this WebApplication app)
    {
        if (Registrar.Registrations == null)
            throw new InvalidOperationException($"Missing endpoint registrar. Did you forget to call {nameof(AddMediatREndoints)}?");

        var registrationGroups = Registrar.Registrations
            .GroupBy(r => r.Group)
            .ToList();

        foreach (var registrationGroup in registrationGroups)
        {
            var groupBuilder = app.MapGroup(registrationGroup.Key);

            Registrar.Configuration.GlobalRouteGroupBuilder(groupBuilder);

            foreach (var registration in registrationGroup.ToList())
            {
                EndpointMapper.MapEndpoint(groupBuilder, registration, Registrar.Configuration.EndpointHandlerDelegateFactory);                
            }

            if (Registrar.Configuration.RouteGroupBuilders.TryGetValue(registrationGroup.Key, out Action<RouteGroupBuilder> builder))
                builder(groupBuilder);                     
        }
    }
}
