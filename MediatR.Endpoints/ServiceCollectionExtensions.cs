using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MediatR.Endpoints;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediatREndoints(this IServiceCollection services, Action<MediatREndpointsConfiguration> configAction = null)
    {
        var assemblies = services
            .Where(a => a.ServiceType.IsGenericType)
            .Where(a => a.ServiceType.GetGenericTypeDefinition() == typeof(IRequestHandler<>)
                || a.ServiceType.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
            .Select(a => a.ImplementationType.Assembly)
            .Distinct()
            .ToArray();

        return services.AddMediatREndoints(assemblies, configAction);
    }
    public static IServiceCollection AddMediatREndoints(this IServiceCollection services, Assembly assembly, Action<MediatREndpointsConfiguration> configAction = null)
    {
        return services.AddMediatREndoints(new[] { assembly }, configAction);
    }

    public static IServiceCollection AddMediatREndoints(this IServiceCollection services, Assembly[] assemblies, Action<MediatREndpointsConfiguration> configAction = null)
    {
        if (!services.Any(a => a.ServiceType == typeof(IMediator)))
            throw new InvalidOperationException("Missing IMediator service. Did you forget to call AddMediatR?");

        var config = new MediatREndpointsConfiguration();

        configAction?.Invoke(config);
        config.RegisterFromAssemblies(assemblies);
        Registrar.Initialize(config);
        return services;
    }

    public static void MapMediatREndpoints(this IEndpointRouteBuilder app)
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
