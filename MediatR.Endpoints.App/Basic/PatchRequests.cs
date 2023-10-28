namespace MediatR.Endpoints.App.Basic;

/******************************************************************************/
[Endpoint(Method.Patch)]
public class PatchRequestMappedByAttributeAllDefaults : IRequest<string> { }
public class PatchHandler1 : IRequestHandler<PatchRequestMappedByAttributeAllDefaults, string>
{
    public Task<string> Handle(PatchRequestMappedByAttributeAllDefaults request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(PatchRequestMappedByAttributeAllDefaults).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
[Endpoint(Method.Patch, "some/custom/patch/route", Group = "someGroup")]
public class PatchRequestMappedByAttributeCustomRouteAndGroup : IRequest<string> { }
public class PatchHandler2 : IRequestHandler<PatchRequestMappedByAttributeCustomRouteAndGroup, string>
{
    public Task<string> Handle(PatchRequestMappedByAttributeCustomRouteAndGroup request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(PatchRequestMappedByAttributeCustomRouteAndGroup).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
public class PatchRequestMappedByConfigAllDefaults : IRequest<string> { }
public class PatchConfig1 : IEndpointConfiguration<PatchRequestMappedByConfigAllDefaults>
{
    public void Configure(IEndpointDefinition e)
    {
        e.HttpMethod = Method.Patch;
    }
}
public class PatchHandler3 : IRequestHandler<PatchRequestMappedByConfigAllDefaults, string>
{
    public Task<string> Handle(PatchRequestMappedByConfigAllDefaults request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(PatchRequestMappedByConfigAllDefaults).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
public class PatchRequestMappedByConfigCustomRouteAndGroupAndBuilder : IRequest<string> { }
public class PatchConfig2 : IEndpointConfiguration<PatchRequestMappedByConfigCustomRouteAndGroupAndBuilder>
{
    public void Configure(IEndpointDefinition e)
    {
        e.HttpMethod = Method.Patch;
        e.Route = "another/custom/patch/route";
        e.Group = "anotherGroup";
        e.RouteHandlerBuilder = a => a.AddEndpointFilter<EndpointFilter1>().AddEndpointFilter<EndpointFilter2>();
    }
}
public class PatchHandler4 : IRequestHandler<PatchRequestMappedByConfigCustomRouteAndGroupAndBuilder, string>
{
    public Task<string> Handle(PatchRequestMappedByConfigCustomRouteAndGroupAndBuilder request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(PatchRequestMappedByConfigCustomRouteAndGroupAndBuilder).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
public class PatchRequestMappedByServicesAllDefaults : IRequest<string> { }
public class PatchHandler5 : IRequestHandler<PatchRequestMappedByServicesAllDefaults, string>
{
    public Task<string> Handle(PatchRequestMappedByServicesAllDefaults request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(PatchRequestMappedByServicesAllDefaults).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
public class PatchRequestMappedByServicesCustomRouteAndGroupAndBuilder : IRequest<string> { }
public class PatchHandler6 : IRequestHandler<PatchRequestMappedByServicesCustomRouteAndGroupAndBuilder, string>
{
    public Task<string> Handle(PatchRequestMappedByServicesCustomRouteAndGroupAndBuilder request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(PatchRequestMappedByServicesCustomRouteAndGroupAndBuilder).FullName + " handled");
}
