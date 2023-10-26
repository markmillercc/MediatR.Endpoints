namespace MediatR.Endpoints;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class EndpointAttribute : Attribute, IEndpointDefinition
{    
    public EndpointAttribute(Method httpMethod, string route = null)
    {
        HttpMethod = httpMethod;
        Route = route;        
    }     
    public Method HttpMethod { get; set; }
    public string Route { get; set; }
    public string Group { get; set; }    
}


