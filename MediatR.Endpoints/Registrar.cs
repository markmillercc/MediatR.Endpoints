using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace MediatR.Endpoints;

internal static class Registrar
{
    public static MediatREndpointsConfiguration Configuration { get; private set; }
    public static IEnumerable<EndpointRegistration> Registrations { get; private set; }

    public static void Initialize(MediatREndpointsConfiguration configuration)
    {
        Configuration = configuration;

        var attributeRegistrations = GetAttributeRegistrations(Configuration.Assemblies);
        var interfaceRegistrations = GetInterfaceRegistrations(Configuration.Assemblies);

        var allRegistrationsGroupedByPath = Configuration.Registrations 
            .Concat(interfaceRegistrations)
            .Concat(attributeRegistrations)
            .Select(r => 
            {
                r.Route ??= Configuration.DefaultRouteFormatter(r.RequestType);
                r.Group ??= "";
                return r;
            })
            .GroupBy(r => new { r.Group, r.Route })
            .ToList();

        var registrations = new List<EndpointRegistration>();

        foreach (var pathGroup in allRegistrationsGroupedByPath)
        {
            var registration = pathGroup.First();

            if (pathGroup.Count() > 1)
            {
                if (pathGroup.Any(a => a.HttpMethod != registration.HttpMethod))
                    throw new InvalidOperationException($"{registration.Group}/{registration.Route}: Ambiguous HttpMethod");

                if (pathGroup.Any(a => a.RequestType != registration.RequestType))
                    throw new InvalidOperationException($"{registration.Group}/{registration.Route}: Ambiguous request type");

                var consolidatedBuilder = new Action<RouteHandlerBuilder>(a => { });

                foreach (var item in pathGroup)
                    consolidatedBuilder += item.RouteHandlerBuilder;
                
                registration.RouteHandlerBuilder = consolidatedBuilder;
            }

            registrations.Add(registration);
        }

        Registrations = registrations;
    }    

    private static IEnumerable<EndpointRegistration> GetInterfaceRegistrations(Assembly[] assemblies)
    {
        var registrations = new List<EndpointRegistration>();

        var models = assemblies.SelectMany(a => a.GetTypes()
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEndpointConfiguration<>))
                .Select(ii => new { ConfigType = t, RequestType = ii.GetGenericArguments()[0] })))
            .ToList();

        foreach (var model in models) 
        {
            var config = (IBaseEndpointConfiguration)Activator.CreateInstance(model.ConfigType);
            var registration = new EndpointRegistration(model.RequestType);
            config.Configure(registration);
            registrations.Add(registration);
        }

        return registrations;
    }

    private static IEnumerable<EndpointRegistration> GetAttributeRegistrations(Assembly[] assemblies)
    {
        var registrations = assemblies.SelectMany(a => a.GetTypes()
            .SelectMany(type => type.GetCustomAttributes<EndpointAttribute>()
                .Select(attr =>
                {
                    if (typeof(IBaseRequest).IsAssignableFrom(type))
                        return new EndpointRegistration(type, attr);
                    else 
                    {
                        var nested = type.GetNestedTypes().Where(typeof(IBaseRequest).IsAssignableFrom).ToList();
                        if (nested.Count != 1)
                            throw new InvalidOperationException($"Missing or ambiguous endpoint request object");

                        return new EndpointRegistration(nested[0], attr);
                    }
                }
                )))
            .ToList();

        return registrations;
    }
}
