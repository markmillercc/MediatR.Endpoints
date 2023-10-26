using Microsoft.AspNetCore.Builder;

namespace MediatR.Endpoints;

internal class EndpointRegistration : IEndpointDefinition
{
    public EndpointRegistration(Type requestType)
    {
        if (!typeof(IBaseRequest).IsAssignableFrom(requestType))
            throw new NotSupportedException($"{requestType} is not a valid request type");

        var genericDefinition = requestType.GetInterfaces()
            .FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IRequest<>));

        if (genericDefinition != null)
            ResponseType = genericDefinition.GetGenericArguments()[0];

        RequestType = requestType;
    }

    public EndpointRegistration(Type requestType, IEndpointDefinition detail) 
        : this(requestType)
    {
        HttpMethod = detail.HttpMethod;
        Route = detail.Route;
        Group = detail.Group;        
        RouteHandlerBuilder = detail.RouteHandlerBuilder;
        Handler = detail.Handler;
    }

    public Type RequestType { get; }
    public Type ResponseType { get; }
    public Method HttpMethod { get; set; }
    public string Route { get; set; }
    public string Group { get; set; }    
    public Action<RouteHandlerBuilder> RouteHandlerBuilder { get; set; }
    public Delegate Handler { get; set; }

}