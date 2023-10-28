namespace MediatR.Endpoints.App.Basic;

/******************************************************************************/
[Endpoint(Method.Get)]
public class GetRequestMappedByAttributeAllDefaults : IRequest<string> { }
public class Handler1 : IRequestHandler<GetRequestMappedByAttributeAllDefaults, string>
{
    public Task<string> Handle(GetRequestMappedByAttributeAllDefaults request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(GetRequestMappedByAttributeAllDefaults).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
[Endpoint(Method.Get, "some/custom/get/route", Group = "someGroup")]
public class GetRequestMappedByAttributeCustomRouteAndGroup : IRequest<string> { }
public class Handler2 : IRequestHandler<GetRequestMappedByAttributeCustomRouteAndGroup, string>
{
    public Task<string> Handle(GetRequestMappedByAttributeCustomRouteAndGroup request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(GetRequestMappedByAttributeCustomRouteAndGroup).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
public class GetRequestMappedByConfigAllDefaults : IRequest<string> { }
public class Config1 : IEndpointConfiguration<GetRequestMappedByConfigAllDefaults>
{
    public void Configure(IEndpointDefinition e)
    {
        e.HttpMethod = Method.Get;
    }
}
public class Handler3 : IRequestHandler<GetRequestMappedByConfigAllDefaults, string>
{
    public Task<string> Handle(GetRequestMappedByConfigAllDefaults request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(GetRequestMappedByConfigAllDefaults).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
public class GetRequestMappedByConfigCustomRouteAndGroupAndBuilder : IRequest<string> { }
public class Config2 : IEndpointConfiguration<GetRequestMappedByConfigCustomRouteAndGroupAndBuilder>
{
    public void Configure(IEndpointDefinition e)
    {
        e.HttpMethod = Method.Get;
        e.Route = "another/custom/get/route";
        e.Group = "anotherGroup";
        e.RouteHandlerBuilder = a => a.AddEndpointFilter<EndpointFilter1>().AddEndpointFilter<EndpointFilter2>();
    }
}
public class Handler4 : IRequestHandler<GetRequestMappedByConfigCustomRouteAndGroupAndBuilder, string>
{
    public Task<string> Handle(GetRequestMappedByConfigCustomRouteAndGroupAndBuilder request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(GetRequestMappedByConfigCustomRouteAndGroupAndBuilder).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
public class GetRequestMappedByServicesAllDefaults : IRequest<string> { }
public class Handler5 : IRequestHandler<GetRequestMappedByServicesAllDefaults, string>
{
    public Task<string> Handle(GetRequestMappedByServicesAllDefaults request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(GetRequestMappedByServicesAllDefaults).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
public class GetRequestMappedByServicesCustomRouteAndGroupAndBuilder : IRequest<string> { }
public class Handler6 : IRequestHandler<GetRequestMappedByServicesCustomRouteAndGroupAndBuilder, string>
{
    public Task<string> Handle(GetRequestMappedByServicesCustomRouteAndGroupAndBuilder request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(GetRequestMappedByServicesCustomRouteAndGroupAndBuilder).FullName + " handled");
}
/******************************************************************************/


