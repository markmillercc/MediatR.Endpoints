namespace MediatR.Endpoints.App.Basic;

/******************************************************************************/
[Endpoint(Method.Delete)]
public class DeleteRequestMappedByAttributeAllDefaults : IRequest<string> { }
public class DeleteHandler1 : IRequestHandler<DeleteRequestMappedByAttributeAllDefaults, string>
{
    public Task<string> Handle(DeleteRequestMappedByAttributeAllDefaults request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(DeleteRequestMappedByAttributeAllDefaults).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
[Endpoint(Method.Delete, "some/custom/delete/route", Group = "someGroup")]
public class DeleteRequestMappedByAttributeCustomRouteAndGroup : IRequest<string> { }
public class DeleteHandler2 : IRequestHandler<DeleteRequestMappedByAttributeCustomRouteAndGroup, string>
{
    public Task<string> Handle(DeleteRequestMappedByAttributeCustomRouteAndGroup request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(DeleteRequestMappedByAttributeCustomRouteAndGroup).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
public class DeleteRequestMappedByConfigAllDefaults : IRequest<string> { }
public class DeleteConfig1 : IEndpointConfiguration<DeleteRequestMappedByConfigAllDefaults>
{
    public void Configure(IEndpointDefinition e)
    {
        e.HttpMethod = Method.Delete;
    }
}
public class DeleteHandler3 : IRequestHandler<DeleteRequestMappedByConfigAllDefaults, string>
{
    public Task<string> Handle(DeleteRequestMappedByConfigAllDefaults request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(DeleteRequestMappedByConfigAllDefaults).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
public class DeleteRequestMappedByConfigCustomRouteAndGroupAndBuilder : IRequest<string> { }
public class DeleteConfig2 : IEndpointConfiguration<DeleteRequestMappedByConfigCustomRouteAndGroupAndBuilder>
{
    public void Configure(IEndpointDefinition e)
    {
        e.HttpMethod = Method.Delete;
        e.Route = "another/custom/delete/route";
        e.Group = "anotherGroup";
        e.RouteHandlerBuilder = a => a.AddEndpointFilter<EndpointFilter1>().AddEndpointFilter<EndpointFilter2>();
    }
}
public class DeleteHandler4 : IRequestHandler<DeleteRequestMappedByConfigCustomRouteAndGroupAndBuilder, string>
{
    public Task<string> Handle(DeleteRequestMappedByConfigCustomRouteAndGroupAndBuilder request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(DeleteRequestMappedByConfigCustomRouteAndGroupAndBuilder).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
public class DeleteRequestMappedByServicesAllDefaults : IRequest<string> { }
public class DeleteHandler5 : IRequestHandler<DeleteRequestMappedByServicesAllDefaults, string>
{
    public Task<string> Handle(DeleteRequestMappedByServicesAllDefaults request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(DeleteRequestMappedByServicesAllDefaults).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
public class DeleteRequestMappedByServicesCustomRouteAndGroupAndBuilder : IRequest<string> { }
public class DeleteHandler6 : IRequestHandler<DeleteRequestMappedByServicesCustomRouteAndGroupAndBuilder, string>
{
    public Task<string> Handle(DeleteRequestMappedByServicesCustomRouteAndGroupAndBuilder request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(DeleteRequestMappedByServicesCustomRouteAndGroupAndBuilder).FullName + " handled");
}
