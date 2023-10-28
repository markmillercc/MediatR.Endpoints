namespace MediatR.Endpoints.App;

public class GlobalEndpointFilter : BaseEndpointFilter { }
public class EndpointFilter1 : BaseEndpointFilter { }
public class EndpointFilter2 : BaseEndpointFilter { }
public class EndpointFilter3 : BaseEndpointFilter { }

public abstract class BaseEndpointFilter : IEndpointFilter
{
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (context.HttpContext.Response.Headers.TryGetValue("EndpointFilters", out var vals))
        {
            var filters = vals.ToList();
            filters.Add(GetType().Name);
            context.HttpContext.Response.Headers["EndpointFilters"] = filters.ToArray();
        }
        else
        {
            context.HttpContext.Response.Headers.Add("EndpointFilters", new string[] { GetType().Name });
        }
        return await next(context);
    }
}

public class MyHandlerFactory : DefaultEndpointHandlerDelegateFactory
{
    public override Delegate GetHandler<TRequest, TResponse>()
        => base.GetHandler<TRequest, TResponse>();
}

public class SomeDependency
{
    public string DoSomething(object obj) => $"Did something to {obj}";
}

