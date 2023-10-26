using Microsoft.AspNetCore.Builder;

namespace MediatR.Endpoints;

public interface IEndpointDefinition
{
    public Method HttpMethod { get; set; }    
    public string Group { get; set; }
    public string Route { get; set; }
    public Action<RouteHandlerBuilder> RouteHandlerBuilder { get => default; set { } }
    public Delegate Handler { get => default; set { } }
}

