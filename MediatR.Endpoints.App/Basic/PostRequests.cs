namespace MediatR.Endpoints.App.Basic;

/******************************************************************************/
[Endpoint(Method.Post)]
public class PostRequestMappedByAttributeAllDefaults : IRequest<string> { }
public class PostHandler1 : IRequestHandler<PostRequestMappedByAttributeAllDefaults, string>
{
    public Task<string> Handle(PostRequestMappedByAttributeAllDefaults request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(PostRequestMappedByAttributeAllDefaults).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
[Endpoint(Method.Post, "some/custom/post/route", Group = "someGroup")]
public class PostRequestMappedByAttributeCustomRouteAndGroup : IRequest<string> { }
public class PostHandler2 : IRequestHandler<PostRequestMappedByAttributeCustomRouteAndGroup, string>
{
    public Task<string> Handle(PostRequestMappedByAttributeCustomRouteAndGroup request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(PostRequestMappedByAttributeCustomRouteAndGroup).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
public class PostRequestMappedByConfigAllDefaults : IRequest<string> { }
public class PostConfig1 : IEndpointConfiguration<PostRequestMappedByConfigAllDefaults>
{
    public void Configure(IEndpointDefinition e)
    {
        e.HttpMethod = Method.Post;
    }
}
public class PostHandler3 : IRequestHandler<PostRequestMappedByConfigAllDefaults, string>
{
    public Task<string> Handle(PostRequestMappedByConfigAllDefaults request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(PostRequestMappedByConfigAllDefaults).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
public class PostRequestMappedByConfigCustomRouteAndGroupAndBuilder : IRequest<string> { }
public class PostConfig2 : IEndpointConfiguration<PostRequestMappedByConfigCustomRouteAndGroupAndBuilder>
{
    public void Configure(IEndpointDefinition e)
    {
        e.HttpMethod = Method.Post;
        e.Route = "another/custom/Post/route";
        e.Group = "anotherGroup";
        e.RouteHandlerBuilder = a => a.AddEndpointFilter<EndpointFilter1>().AddEndpointFilter<EndpointFilter2>();
    }
}
public class PostHandler4 : IRequestHandler<PostRequestMappedByConfigCustomRouteAndGroupAndBuilder, string>
{
    public Task<string> Handle(PostRequestMappedByConfigCustomRouteAndGroupAndBuilder request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(PostRequestMappedByConfigCustomRouteAndGroupAndBuilder).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
public class PostRequestMappedByServicesAllDefaults : IRequest<string> { }
public class PostHandler5 : IRequestHandler<PostRequestMappedByServicesAllDefaults, string>
{
    public Task<string> Handle(PostRequestMappedByServicesAllDefaults request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(PostRequestMappedByServicesAllDefaults).FullName + " handled");
}
/******************************************************************************/

/******************************************************************************/
public class PostRequestMappedByServicesCustomRouteAndGroupAndBuilder : IRequest<string> { }
public class PostHandler6 : IRequestHandler<PostRequestMappedByServicesCustomRouteAndGroupAndBuilder, string>
{
    public Task<string> Handle(PostRequestMappedByServicesCustomRouteAndGroupAndBuilder request, CancellationToken cancellationToken)
        => Task.FromResult(typeof(PostRequestMappedByServicesCustomRouteAndGroupAndBuilder).FullName + " handled");
}
