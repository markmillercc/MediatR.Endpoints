namespace MediatR.Endpoints.App.Basic;

/******************************************************************************/
[Endpoint(Method.Put)]
public class PutRequestMappedByAttributeAllDefaults : IRequest<string> { }
public class PutHandler1 : IRequestHandler<PutRequestMappedByAttributeAllDefaults, string>
{
    public Task<string> Handle(PutRequestMappedByAttributeAllDefaults request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(PutRequestMappedByAttributeAllDefaults).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
[Endpoint(Method.Put, "some/custom/put/route", Group = "someGroup")]
public class PutRequestMappedByAttributeCustomRouteAndGroup : IRequest<string> { }
public class PutHandler2 : IRequestHandler<PutRequestMappedByAttributeCustomRouteAndGroup, string>
{
    public Task<string> Handle(PutRequestMappedByAttributeCustomRouteAndGroup request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(PutRequestMappedByAttributeCustomRouteAndGroup).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
public class PutRequestMappedByConfigAllDefaults : IRequest<string> { }
public class PutConfig1 : IEndpointConfiguration<PutRequestMappedByConfigAllDefaults>
{
    public void Configure(IEndpointDefinition e)
    {
        e.HttpMethod = Method.Put;
    }
}
public class PutHandler3 : IRequestHandler<PutRequestMappedByConfigAllDefaults, string>
{
    public Task<string> Handle(PutRequestMappedByConfigAllDefaults request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(PutRequestMappedByConfigAllDefaults).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
public class PutRequestMappedByConfigCustomRouteAndGroupAndBuilder : IRequest<string> { }
public class PutConfig2 : IEndpointConfiguration<PutRequestMappedByConfigCustomRouteAndGroupAndBuilder>
{
    public void Configure(IEndpointDefinition e)
    {
        e.HttpMethod = Method.Put;
        e.Route = "another/custom/put/route";
        e.Group = "anotherGroup";
        e.RouteHandlerBuilder = a => a.AddEndpointFilter<EndpointFilter1>().AddEndpointFilter<EndpointFilter2>();
    }
}
public class PutHandler4 : IRequestHandler<PutRequestMappedByConfigCustomRouteAndGroupAndBuilder, string>
{
    public Task<string> Handle(PutRequestMappedByConfigCustomRouteAndGroupAndBuilder request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(PutRequestMappedByConfigCustomRouteAndGroupAndBuilder).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
public class PutRequestMappedByServicesAllDefaults : IRequest<string> { }
public class PutHandler5 : IRequestHandler<PutRequestMappedByServicesAllDefaults, string>
{
    public Task<string> Handle(PutRequestMappedByServicesAllDefaults request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(PutRequestMappedByServicesAllDefaults).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
public class PutRequestMappedByServicesCustomRouteAndGroupAndBuilder : IRequest<string> { }
public class PutHandler6 : IRequestHandler<PutRequestMappedByServicesCustomRouteAndGroupAndBuilder, string>
{
    public Task<string> Handle(PutRequestMappedByServicesCustomRouteAndGroupAndBuilder request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(PutRequestMappedByServicesCustomRouteAndGroupAndBuilder).FullName + " handled");
}
